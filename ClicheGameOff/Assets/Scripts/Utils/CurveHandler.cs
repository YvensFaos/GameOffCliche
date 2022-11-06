using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class CurveHandler
    {
        [SerializeField]
        private AnimationCurve curve;
        public float GetMaxLevel() => curve.length;

        public float EvaluateAtLevel(float level, out bool maxLevel)
        {
            maxLevel = level < GetMaxLevel();
            return maxLevel ? -1 : curve.Evaluate(level);
        } 
    }
}