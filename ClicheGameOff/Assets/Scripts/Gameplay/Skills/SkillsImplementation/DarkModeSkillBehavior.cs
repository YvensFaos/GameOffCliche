using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Skills.SkillsImplementation
{
    public class DarkModeSkillBehavior : MonoBehaviour
    {
        [SerializeField]
        private Volume volume;

        [SerializeField] private float transitionTime;

        private void Awake()
        {
            volume.weight = 0.0f;
        }
        
        public void Activate(float time)
        {
            StartCoroutine(DarkModeCoroutine(time));
        }

        private IEnumerator DarkModeCoroutine(float time)
        {
            DOTween.To(() => volume.weight, value => volume.weight = value, 1.0f, transitionTime);
            yield return new WaitForSeconds(time - 2 * transitionTime);
            DOTween.To(() => volume.weight, value => volume.weight = value, 0.0f, transitionTime);
        }
    }
}
