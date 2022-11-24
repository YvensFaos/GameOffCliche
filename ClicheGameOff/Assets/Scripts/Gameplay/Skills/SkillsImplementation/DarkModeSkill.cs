using UnityEngine;
using DG.Tweening;
using Events;

namespace Gameplay.Skills.SkillsImplementation
{
    public class DarkModeSkill : PlayerSkill
    {
        [Header("Skill Settings")]
        [SerializeField] private DarkModeSkillBehavior darkModeObject;
        [SerializeField] private float darkModeDuration;

        protected override void SkillEffect(in PlayerController playerController)
        {
            // Shows spawned data movement type
            GameManager.Instance.MainSpawner.GetCurrentDataList.ForEach(spawnedData => 
            {
                if (spawnedData != null)
                {
                    spawnedData.RevealDataType();    
                }
            });

            //Spawn VFX
            var darkObject = Instantiate(darkModeObject);
            darkObject.Activate(darkModeDuration);
            Destroy(darkObject.gameObject, darkModeDuration);
        }
    }
}