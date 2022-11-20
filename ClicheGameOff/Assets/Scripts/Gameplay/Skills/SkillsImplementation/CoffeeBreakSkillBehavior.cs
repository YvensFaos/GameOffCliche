using Data;
using UnityEngine;
using Utils;

namespace Gameplay.Skills.SkillsImplementation
{
    public class CoffeeBreakSkillBehavior : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _coffeeBreakParticles;

        public void Initialize(float coffeeBreakDuration)
        {
            if(!this._coffeeBreakParticles)
                Debug.LogWarning($"CoffeeBreakSkillBehavior has no particle system attached");
        }
    }
}
