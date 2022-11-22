using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

namespace Data
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseDataBehavior : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] protected AnimationCurve scaleCurve;
        [SerializeField] protected float currentScaleFactor = 1.0f;
        [SerializeField] private float minimalScale = 0.1f;
        [SerializeField] private float maximalScale = 1.0f;
        [SerializeField] private float startCurveValue = 0.0f;
        [SerializeField] private float curveStepFactor = 0.1f;
        [SerializeField] private LayerMask collisionLayer;
        
        [Header("Behavior")]
        [SerializeField] private BoxCollider walkableArea;
        [SerializeField] private LayerMask walkableLayer;
        [SerializeField] private float actTimer;
        [SerializeField] private float actTimerFluctuation;
        [SerializeField] private float stopDistance;
        //TODO add animator

        [Header("Attributes")] 
        [SerializeField] private DataType type;
        [SerializeField] private float regularSpeed;
        [SerializeField] private int hardDriveUse = 1;
        
        [Header("References")] 
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Renderer selfRenderer;

        [Header("Types")]
        [SerializeField] private List<GameUpgradeMaterialPair> materials;

        //Private
        private float scaleTimeStamp;
        private bool beingMined;
        private bool wasCollected;
        private bool stuck;
        private float minimalScaleValue;
        private float maximalScaleValue;

        private void Awake()
        {
            if (selfRenderer == null)
            {
                selfRenderer = GetComponent<Renderer>();
            }
        }
        
        private void OnEnable()
        {
            currentScaleFactor = scaleCurve.Evaluate(startCurveValue);
            scaleTimeStamp = startCurveValue;

            minimalScale = scaleCurve.Evaluate(scaleCurve.length);
            maximalScale = scaleCurve.Evaluate(0);

            navMeshAgent.speed = regularSpeed;
        }

        public void Initialize(BoxCollider area, DataType dataType)
        {
            walkableArea = area;
            type = dataType;
            name = $"{name}-{dataType.GetName()}";
            
            //Calculate material type given the player knowledge
            var upgrade = materials.Find(pair => pair.One.RequiredData == type.qualifier);
            if (upgrade != null)
            {
                var chance = GameManager.Instance.GetCurrentUpgradeValue(upgrade.One);
                //Try to change the material to be the specific material for this upgrade
                if (RandomChanceUtils.GetChance(chance))
                {
                    selfRenderer.material = upgrade.Two;
                }
            }
            
            StartCoroutine(DataCoroutine());
        }
        
        IEnumerator DataCoroutine()
        {
            while (!wasCollected && !stuck)
            {
                navMeshAgent.isStopped = true;
                yield return Act(); 
                yield return new WaitForSeconds(actTimer + Random.Range(-actTimerFluctuation, actTimerFluctuation));
            }
        }

        protected virtual IEnumerator Act()
        {
            // ReSharper disable once RedundantAssignment
            var validPoint = transform.position;
            
            //The safe check is used to avoid the do/while getting into an infinite loop trying to get a valid position.
            var safeCheck = 10;
            do
            {
                validPoint = RandomPointUtils.GetRandomPointWithBox(walkableArea);
            } while (safeCheck-- > 0 &&
                     !CollisionUtils.GetValidPointInLayer(validPoint, Vector3.down, 30.0f, walkableLayer,
                         out validPoint));

            if (safeCheck <= 0)
            {
                //This data entity is stuck somewhere and cannot find a valid point
                stuck = true;
            }
            
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = validPoint;
            yield return new WaitUntil(() => navMeshAgent != null && navMeshAgent.remainingDistance <= stopDistance);
        }

        private void Collect()
        {
            wasCollected = true;
            GameManager.Instance.MainRunner.CollectData(this);
            StopAllCoroutines();
            var particlesTransform = particles.transform;
            particlesTransform.parent = transform.parent;
            particlesTransform.localScale = Vector3.one;
            particles.Stop();

            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider collision)
        {
            if (!CollisionUtils.CheckLayerCollision(collisionLayer, collision.gameObject)) return;
            Shrink();
        }

        private void Shrink()
        {
            //The curve goes from 0 (regular size) to 1 (minimal size)
            scaleTimeStamp = Mathf.Clamp(scaleTimeStamp + curveStepFactor * (100.0f * Time.deltaTime), minimalScale, maximalScale);
            currentScaleFactor = Mathf.Clamp( scaleCurve.Evaluate(scaleTimeStamp), minimalScale, maximalScale);
            UpdateSize();

            if (currentScaleFactor <= minimalScale)
            {
                Collect();
            }
        }

        private void UpdateSize()
        {
            transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);
            particles.gameObject.transform.localScale = Vector3.one;
        }
        
        public DataType Type => type;

        public int HardDriveUse => hardDriveUse;

        #region Skills Behaviour Related

        public void StopAgentMovement()
        {
            this.navMeshAgent.speed = 0.0f;
            this.navMeshAgent.velocity = Vector3.zero;
        }

        public void ReturnAgentMovement()
        {
            this.navMeshAgent.speed = this.regularSpeed;
        }

        #endregion
    }
}