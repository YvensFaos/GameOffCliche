using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

namespace Gameplay.Skills.SkillsImplementation
{   
    [RequireComponent(typeof(SphereCollider))]
    public class LaTexBindBehavior : MonoBehaviour
    {
        [SerializeField] private float influenceSphereRadius = 1.2f;
        [SerializeField] private GameObject sphereMesh;
        [SerializeField] private ParticleSystem laTexBindParticles;
        [SerializeField] private LayerMask dataLayer;
        [SerializeField] private GameObject slowdownLaTexBindParticles;

        private Dictionary<BaseDataBehavior, GameObject> instantiatedParticleEffectsDictionary;
        private SphereCollider sphereCollider;

        private PlayerController playerController;
        private float slowPercentage;

        public void Initialize(float laTexBindDuration, float slowPercentage, in PlayerController controller)
        {
            playerController = controller;

            laTexBindParticles.Stop();
            var laTexBindParticlesMain = laTexBindParticles.main;
            laTexBindParticlesMain.duration = laTexBindDuration;
            laTexBindParticles.Play();
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            sphereCollider.radius = influenceSphereRadius;
            sphereMesh.transform.localScale = Vector3.one * (2 * influenceSphereRadius);
            instantiatedParticleEffectsDictionary = new Dictionary<BaseDataBehavior, GameObject>();
        }

        private void Update()
        {
            if(playerController)
                transform.position = playerController.LastValidHit;
        }

        private void OnDestroy() 
        {   
            foreach (KeyValuePair<BaseDataBehavior, GameObject> item in instantiatedParticleEffectsDictionary)
            {
                item.Key.ReturnAgentMovement();
                Destroy(item.Value);
            }

            instantiatedParticleEffectsDictionary.Clear();
        }   

        #endregion

        #region Events Methods

        private void OnTriggerEnter(Collider other)
        {
            SolveEnter(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            SolveEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            SolveExit(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            SolveExit(other.gameObject);
        }

        private void SolveEnter(GameObject other)
        {
            if (!CollisionUtils.CheckLayerCollision(dataLayer, other)) return;

            var otherTransform = other.transform;
            var dataBehavior = other.GetComponent<BaseDataBehavior>();
            if (dataBehavior == null || dataBehavior.Type.qualifier != DataQualifier.Good) return;

            //Slowdown
            dataBehavior.SetAgentMovement(dataBehavior.GetNavMeshAgentSpeed * slowPercentage);

            //VFX
            instantiatedParticleEffectsDictionary.Add(dataBehavior, Instantiate(slowdownLaTexBindParticles, otherTransform.position, otherTransform.rotation));
        }

        private void SolveExit(GameObject other)
        {
            if (!CollisionUtils.CheckLayerCollision(dataLayer, other)) return;

            var otherTransform = other.transform;
            var dataBehavior = other.GetComponent<BaseDataBehavior>();
            if (dataBehavior == null || dataBehavior.Type.qualifier != DataQualifier.Good) return;

            //Slowdown
            dataBehavior.ReturnAgentMovement();

            //VFX
            GameObject slowdownParticleEffectGO;
            if(instantiatedParticleEffectsDictionary.TryGetValue(dataBehavior, out slowdownParticleEffectGO))
            {
                instantiatedParticleEffectsDictionary.Remove(dataBehavior);
                Destroy(slowdownParticleEffectGO);
            }
        }

        #endregion
    }
}
