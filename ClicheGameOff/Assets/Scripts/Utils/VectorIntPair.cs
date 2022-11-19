using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class VectorIntPair : Pair<Vector3, int>
    {
        public VectorIntPair(Vector3 one, int two) : base(one, two)
        { }
    }
}