using System;
using UnityEngine;
using UnityEngine.AI;

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

        [Header("References")] [SerializeField]
        protected NavMeshAgent navMeshAgent;

        //Private
        private float scaleTimeStamp;
        private bool beingMined;

        private float minimalScaleValue;
        private float maximalScaleValue;

        public void OnEnable()
        {
            currentScaleFactor = scaleCurve.Evaluate(startCurveValue);
            scaleTimeStamp = startCurveValue;

            minimalScale = scaleCurve.Evaluate(scaleCurve.length);
            maximalScale = scaleCurve.Evaluate(0);
        }

        // public void Initialize()
        // {
        // }
        //
        // public void Act()
        // {
        // }

        private void Update()
        {
            if (beingMined)
            {
                //Checking if it shrink down to the minimal
                if (Shrink())
                {
                    //Collect data!
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
            Debug.Log("1");
            if (!(currentScaleFactor > minimalScale)) return currentScaleFactor <= minimalScale;
            
            Debug.Log($"2");
            //The curve goes from 0 (regular size) to 1 (minimal size)
            scaleTimeStamp = Mathf.Clamp(scaleTimeStamp + curveStepFactor, minimalScale, maximalScale);
            Debug.Log($"3 {scaleTimeStamp}");
            currentScaleFactor = Mathf.Clamp( scaleCurve.Evaluate(scaleTimeStamp), minimalScale, maximalScale);
            Debug.Log($"4 {currentScaleFactor}");
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
        }

        private void UpdateSize()
        {
            transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);
        }
    }
}