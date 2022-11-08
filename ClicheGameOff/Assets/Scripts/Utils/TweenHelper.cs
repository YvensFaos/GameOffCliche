using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class TweenHelper
    {
        public static void AnimateMaterial(Renderer renderer, Material start, Material finish, float time)
        {
            var step = 0.0f;
            DOTween.To(() => step, value =>
            {
                step = value;
                renderer.material.Lerp(start, finish, step);
            }, 1.0f, time);
        }
    }
}