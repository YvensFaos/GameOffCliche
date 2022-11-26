using UnityEngine;
using DG.Tweening;
using Events;

namespace Gameplay.Skills.SkillsImplementation
{
    public class CoffeeBreakSkill : PlayerSkill
    {
        [Header("Skill Settings")]
        [SerializeField] private CoffeeBreakSkillBehavior coffeeBreakObject;
        [SerializeField] private float coffeeBreakDuration;
        
        [Header("Scriptable Objects Refs")]
        [SerializeField] private GameplayEventsSO gameplayEventsSo;

        private float timeSinceEffectStarted;

        protected override void SkillEffect(in PlayerController playerController)
        {
            // Starts listening for new data created
            gameplayEventsSo.OnNewDataCreated += HandleOnNewDataCreated;

            timeSinceEffectStarted = Time.time;

            // Pause all spawned data movement
            GameManager.Instance.MainSpawner.GetCurrentDataList.ForEach(spawnedData => 
            {
                spawnedData.SetAgentMovement(0.0f);
                DOVirtual.DelayedCall(coffeeBreakDuration, () => 
                {
                    if (spawnedData != null)
                    {
                        spawnedData.ReturnAgentMovement();
                    }
                });
            });

            //Spawn VFX (global or for each data on scene?)
            var breakObject = Instantiate(coffeeBreakObject);
            breakObject.Initialize(coffeeBreakDuration);
            Destroy(breakObject.gameObject, coffeeBreakDuration);

            //Stop listening for new data created
            DOVirtual.DelayedCall(coffeeBreakDuration, () => 
            {
                gameplayEventsSo.OnNewDataCreated -= HandleOnNewDataCreated;
            });        
        }

        private void HandleOnNewDataCreated(Data.BaseDataBehavior newData)
        {
            var elapsedCooldownTime = Time.time - timeSinceEffectStarted;
            newData.SetAgentMovement(0.0f);
            DOVirtual.DelayedCall(Mathf.Max(0.0f, coffeeBreakDuration - elapsedCooldownTime) , () => 
            {
                if (newData != null)
                {
                    newData.ReturnAgentMovement();
                }
            });
        }
    }
}