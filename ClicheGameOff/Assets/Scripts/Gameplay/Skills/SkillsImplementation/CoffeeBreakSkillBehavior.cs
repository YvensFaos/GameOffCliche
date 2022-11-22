using UnityEngine;

namespace Gameplay.Skills.SkillsImplementation
{
    public class CoffeeBreakSkillBehavior : MonoBehaviour
    {
        [SerializeField] private ParticleSystem coffeeBreakParticles;
        [SerializeField] private Vector3 coffeePosition;

        public void Initialize(float coffeeBreakDuration)
        {
            var coffeeParticles = Instantiate(coffeeBreakParticles, coffeePosition, Quaternion.identity);
            coffeeParticles.Play(true);
            Destroy(coffeeParticles.gameObject, coffeeBreakDuration);
        }
    }
}
