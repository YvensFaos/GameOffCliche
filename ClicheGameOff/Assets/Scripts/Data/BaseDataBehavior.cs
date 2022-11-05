using System.Collections;
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
        [SerializeField] private bool canGrowBack;
        [SerializeField] private LayerMask collisionLayer;
        
        [Header("Behavior")]
        [SerializeField] private BoxCollider walkableArea;
        [SerializeField] private LayerMask walkableLayer;
        [SerializeField] private float actTimer;
        [SerializeField] private float actTimerFluctuation;
        [SerializeField] private float stopDistance;

        [Header("Attributes")] 
        [SerializeField] private DataType type;
        [SerializeField] private float minedSpeed;
        [SerializeField] private float regularSpeed;
        [SerializeField] private int hardDriveUse = 1;
        
        [Header("References")] 
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] private ParticleSystem particles;

        //Private
        private float scaleTimeStamp;
        private bool beingMined;
        private bool wasCollected;
        private bool stuck;

        private float minimalScaleValue;
        private float maximalScaleValue;
        
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

        private void Update()
        {
            if (wasCollected || stuck) return;
            
            if (beingMined)
            {
                //Checking if it shrink down to the minimal
                if (Shrink())
                {
                    //Collect data!
                    Collect();
                }
            }
            else
            {
                //If can grow back, then proceed on getting back to its normal size
                if (canGrowBack)
                {
                    //Grow back to its original size
                    Grown();
                }
            }
        }

        private void Collect()
        {
            wasCollected = true;
            GameManager.Instance.CurrentRun.CollectData(this);
            StopAllCoroutines();
            var particlesTransform = particles.transform;
            particlesTransform.parent = transform.parent;
            particlesTransform.localScale = Vector3.one;
            particles.Stop();

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (CollisionUtils.CheckLayerCollision(collisionLayer, collision.gameObject))
            {
                ToggleMining(true);
            }
        }
        
        private void OnTriggerExit(Collider collision)
        {
            if (CollisionUtils.CheckLayerCollision(collisionLayer, collision.gameObject))
            {
                ToggleMining(false);
            }
        }

        private bool Shrink()
        {
            if (!(currentScaleFactor > minimalScale)) return currentScaleFactor <= minimalScale;
            
            //The curve goes from 0 (regular size) to 1 (minimal size)
            scaleTimeStamp = Mathf.Clamp(scaleTimeStamp + curveStepFactor, minimalScale, maximalScale);
            currentScaleFactor = Mathf.Clamp( scaleCurve.Evaluate(scaleTimeStamp), minimalScale, maximalScale);
            UpdateSize();

            return currentScaleFactor <= minimalScale;
        }

        private void Grown()
        {
            if (!(currentScaleFactor < maximalScale)) return;
            
            //The curve goes from 0 (regular size) to 1 (minimal size)
            scaleTimeStamp = Mathf.Clamp(scaleTimeStamp - curveStepFactor, minimalScale, maximalScale);
            currentScaleFactor = Mathf.Clamp( scaleCurve.Evaluate(scaleTimeStamp), minimalScale, maximalScale);
            UpdateSize();
        }

        private void ToggleMining(bool mining)
        {
            beingMined = mining;
            navMeshAgent.speed = (beingMined) ? regularSpeed : minedSpeed;
        }

        private void UpdateSize()
        {
            transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);
            particles.gameObject.transform.localScale = Vector3.one;
        }
        
        public DataType Type => type;

        public int HardDriveUse => hardDriveUse;
    }
}