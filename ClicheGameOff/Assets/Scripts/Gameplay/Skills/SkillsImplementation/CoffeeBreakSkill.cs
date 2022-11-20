using UnityEngine;
using DG.Tweening;
using Gameplay.Events;

namespace Gameplay.Skills.SkillsImplementation
{
    public class CoffeeBreakSkill : PlayerSkill
    {
        [Header("Skill Settings")]
        [SerializeField] private CoffeeBreakSkillBehavior _coffeeBreakObject;
        [SerializeField] private float _coffeeBreakDuration;
        
        [Header("Scriptable Objects Refs")]
        [SerializeField] private GameplayEventsSO _gameplayEventsSO;

        private float _timeSinceEffectStarted;

        protected override void SkillEffect(in PlayerController playerController)
        {
            // Starts listening for new data created
            this._gameplayEventsSO.OnNewDataCreated += this.HandleOnNewDataCreated;

            this._timeSinceEffectStarted = Time.time;

            // Pause all spawned data movement
            GameManager.Instance.MainSpawner.GetCurrentDataList.ForEach(spawnedData => 
            {
                spawnedData.StopAgentMovement();
                DOVirtual.DelayedCall(this._coffeeBreakDuration, () => 
                {
                    if(spawnedData)
                        spawnedData.ReturnAgentMovement();
                });
            });

            //Spawn VFX (global or for each data on scene?)
            CoffeeBreakSkillBehavior coffeBreakObject = Instantiate(this._coffeeBreakObject);
            coffeBreakObject.Initialize(this._coffeeBreakDuration);
            Destroy(coffeBreakObject.gameObject, this._coffeeBreakDuration);

            //Stop listening for new data created
            DOVirtual.DelayedCall(this._coffeeBreakDuration, () => 
            {
                this._gameplayEventsSO.OnNewDataCreated -= this.HandleOnNewDataCreated;
            });        
        }

        private void HandleOnNewDataCreated(Data.BaseDataBehavior newData)
        {
            float elapsedCooldownTime = Time.time - this._timeSinceEffectStarted;
            newData.StopAgentMovement();
            DOVirtual.DelayedCall(Mathf.Max(0.0f, this._coffeeBreakDuration - elapsedCooldownTime) , () => 
            {
                if(newData)
                    newData.ReturnAgentMovement();
            });
        }
    }
}