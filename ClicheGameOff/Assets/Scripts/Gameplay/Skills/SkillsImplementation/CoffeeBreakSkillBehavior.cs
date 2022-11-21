using Data;
using UnityEngine;
using Utils;

namespace Gameplay.Skills.SkillsImplementation
{
    public class CoffeeBreakSkillBehavior : MonoBehaviour
    {
        [SerializeField] private ParticleSystem coffeeBreakParticles;

        public void Initialize(float coffeeBreakDuration)
        {
            if(!coffeeBreakParticles)
                Debug.LogWarning($"CoffeeBreakSkillBehavior has no particle system attached");
        }
    }
}
