using System;
using UnityEngine;
using Utils;

namespace Gameplay
{
    [RequireComponent(typeof(Renderer))]
    public class FloorController : MonoBehaviour
    {
        [SerializeField] private Renderer selfRenderer;
        [SerializeField] private Material regularMaterial;
        [SerializeField] private Material activateMaterial;

        private void Awake()
        {
            if (selfRenderer == null)
            {
                selfRenderer = GetComponent<Renderer>();
            }
        }

        private void Start()
        {
            ResetMaterial();
        }

        private void ResetMaterial()
        {
            selfRenderer.material = regularMaterial;
        }

        public void ActivateFloor(float time)
        {
            TweenHelper.AnimateMaterial(selfRenderer, regularMaterial, activateMaterial, time);
        }

        public void RegularFloor(float time)
        {
            TweenHelper.AnimateMaterial(selfRenderer, activateMaterial, regularMaterial, time);
        }
    }
}