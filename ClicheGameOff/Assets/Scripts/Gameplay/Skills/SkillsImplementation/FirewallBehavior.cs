using Data;
using UnityEngine;
using Utils;

namespace Gameplay.Skills.SkillsImplementation
{
    public class FirewallBehavior : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem firewallParticles;
        [SerializeField]
        private LayerMask dataLayer;
        [SerializeField]
        private GameObject destructionFirewallParticles;

        public void Initialize(float firewallDuration)
        {
            var firewallParticlesMain = firewallParticles.main;
            firewallParticlesMain.duration = firewallDuration;
            firewallParticles.Play();
        }
    
        private void Start()
        {
            firewallParticles.transform.SetParent(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            Solve(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            Solve(other.gameObject);
        }

        private void Solve(GameObject other)
        {
            if (!CollisionUtils.CheckLayerCollision(dataLayer, other)) return;
            var otherTransform = other.transform;
            var dataBehavior = other.GetComponent<BaseDataBehavior>();
            if (dataBehavior == null || dataBehavior.Type.qualifier != DataQualifier.Bad) return;
            Destroy(other.gameObject);
            Instantiate(destructionFirewallParticles, otherTransform.position, otherTransform.rotation);
        }
    }
}
