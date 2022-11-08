using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Utils
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private Renderer selfRenderer;
        [SerializeField] private List<MaterialNamePair> materials;

        private void Awake()
        {
            if (selfRenderer == null)
            {
                selfRenderer = GetComponent<Renderer>();
            }
        }
        
        [YarnCommand("changeMaterial")]
        public void ChangeMaterial(string materialName, float time)
        {
            var material = materials.Find(pair => pair.One.Equals(materialName))?.Two;
            if (material != null)
            {
                TweenHelper.AnimateMaterial(selfRenderer, selfRenderer.material, material, time);
            }
        }
    }
}
