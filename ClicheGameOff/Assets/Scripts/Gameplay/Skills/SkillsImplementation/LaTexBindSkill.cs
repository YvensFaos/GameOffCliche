using UnityEngine;

namespace Gameplay.Skills.SkillsImplementation
{
    public class LaTexBindSkill : PlayerSkill
    {
        [SerializeField] private LaTexBindBehavior laTexBindObject;
        [SerializeField] private float laTexBindDuration;
        [SerializeField] private float slowPercentage;
        
        protected override void SkillEffect(in PlayerController playerController)
        {
            var playerTransform = playerController.transform;
            var laTexBind = Instantiate(laTexBindObject, playerTransform.position, playerTransform.rotation, playerTransform);
            laTexBind.Initialize(laTexBindDuration, slowPercentage, playerController);
            Destroy(laTexBind.gameObject, laTexBindDuration);
        }
    }
}