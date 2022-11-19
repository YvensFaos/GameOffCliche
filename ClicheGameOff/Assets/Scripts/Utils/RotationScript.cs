using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class RotationScript : MonoBehaviour
    {
        [SerializeField]
        private List<VectorIntPair> axesAndAngle;

        private void Update()
        {
            foreach (var vectorIntPair in axesAndAngle)
            {
                transform.Rotate(vectorIntPair.One, vectorIntPair.Two, Space.Self);
            }
        }
    }
}