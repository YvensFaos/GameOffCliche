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
        [SerializeField] private float dataHeight;
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
        [SerializeField] private float regularMiningRate = 100.0f;
        [SerializeField] private int hardDriveUse = 1;
        
        [Header("References")] 
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystem captureParticles;
        [SerializeField] private Renderer selfRenderer;

        [Header("Types")]
        [SerializeField] private List<GameUpgradeMaterialPair> materials;

        //Public
        public float GetNavMeshAgentSpeed => navMeshAgent.speed;

        //Private
        private float scaleTimeStamp;
        private bool beingMined;
        private bool wasCollected;
        private bool stuck;
        private float minimalScaleValue;
        private float maximalScaleValue;
        private float defaultParticleEmissionRate;

        private void Awake()
        {
            if (selfRenderer == null)
            {
                selfRenderer = GetComponent<Renderer>();
            }
        }

        private void Start()
        {
            defaultParticleEmissionRate = particles.emission.rateOverTime.constant;
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
            if (upgrade == null) return;
            
            var chance = GameManager.Instance.GetCurrentUpgradeValue(upgrade.One);
            //Try to change the material to be the specific material for this upgrade
            if (RandomChanceUtils.GetChance(chance))
            {
                selfRenderer.material = upgrade.Two;
            }

            navMeshAgent.enabled = false;
        }

        public void StartBehavior()
        {
            navMeshAgent.enabled = true;
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
            DisableData();
            var particlesTransform = particles.transform;
            particlesTransform.parent = transform.parent;
            ChangeParticleEmissionRateTo(1.0f);
            particlesTransform.localScale = Vector3.one;
            particles.Stop();

            Destroy(gameObject);

            Instantiate(captureParticles, transform.position, Quaternion.identity);
        }

        public void DisableData()
        {
            StopAllCoroutines();
            navMeshAgent.enabled = false;
        }

        private void OnTriggerStay(Collider collision)
        {
            if (!CollisionUtils.CheckLayerCollision(collisionLayer, collision.gameObject)) return;
            Shrink();
        }

        private void Shrink()
        {
            var miningRate = regularMiningRate * GameManager.Instance.CurrentPlayerData.MiningRate * Time.deltaTime;

            //The curve goes from 0 (regular size) to 1 (minimal size)
            scaleTimeStamp = Mathf.Clamp(scaleTimeStamp + curveStepFactor * miningRate, minimalScale, maximalScale);
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
            ChangeParticleEmissionRateTo(defaultParticleEmissionRate * currentScaleFactor);
        }

        private void ChangeParticleEmissionRateTo(float changeTo)
        {
            var particlesEmission = particles.emission;
            var emissionRateOverTime = particlesEmission.rateOverTime;
            emissionRateOverTime.constant = changeTo;
            particlesEmission.rateOverTime = emissionRateOverTime;
            particles.gameObject.transform.localScale = Vector3.one;
        }

        #region Skills Behaviour Related
        public void SetAgentMovement(float speed)
        {
            navMeshAgent.speed = speed;

            if (speed == 0.0f)
            {
                navMeshAgent.velocity = Vector3.zero;
            }
        }

        public void ReturnAgentMovement()
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = regularSpeed;    
            }
        }

        public void RevealDataType()
        {
            //Calculate material type given the player knowledge
            var upgrade = materials.Find(pair => pair.One.RequiredData == type.qualifier);
            if (upgrade == null) return;

            selfRenderer.material = upgrade.Two;
        }

        #endregion
        
        public DataType Type => type;
        public int HardDriveUse => hardDriveUse;
        public float DataHeight => dataHeight;
    }
}