using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class MaterialNamePair : Pair<string, Material>
    {
        public MaterialNamePair(string one, Material two) : base(one, two)
        { }
    }
}