using UnityEngine;

namespace Gameplay.Skills.SkillsImplementation
{
    public class FirewallBehavior : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem firewallParticles;

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
    }
}
