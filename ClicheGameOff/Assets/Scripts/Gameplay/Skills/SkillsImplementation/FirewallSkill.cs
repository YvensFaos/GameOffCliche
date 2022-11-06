using UnityEngine;

namespace Gameplay.Skills.SkillsImplementation
{
    public class FirewallSkill : PlayerSkill
    {
        [SerializeField]
        private GameObject firewallObject;
        [SerializeField] 
        private float firewallDuration;
        
        protected override void SkillEffect(in PlayerController playerController)
        {
            var playerTransform = playerController.transform;
            var firewall = Instantiate(firewallObject, playerController.LastValidHit, playerTransform.rotation);
            Destroy(firewall, firewallDuration);
        }
    }
}