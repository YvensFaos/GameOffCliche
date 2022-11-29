using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

namespace Gameplay.Skills.SkillsImplementation
{
    public class FirewallBehavior : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> firewallParticlesSystems;
        [SerializeField]
        private LayerMask dataLayer;
        [SerializeField]
        private GameObject destructionFirewallParticles;

        public void Initialize(float firewallDuration)
        {
            firewallParticlesSystems.ForEach(firewall =>
            {
                var firewallParticlesMain = firewall.main;
                firewallParticlesMain.duration = firewallDuration;
                firewall.Play();
            });
        }
    
        private void Start()
        {
            firewallParticlesSystems.ForEach(firewall =>
            {
                firewall.transform.SetParent(null);
            });
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
