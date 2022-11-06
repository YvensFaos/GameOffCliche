using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "New Game Upgrade", menuName = "Cliche/Game Upgrade", order = 0)]
    public class GameUpgrade : ScriptableObject
    {
        [SerializeField, TextArea]
        private string description;
        [SerializeField]
        private bool isProgressUpdate;
        [SerializeField] 
        private CurveHandler progressCurve;
        [SerializeField] 
        private DataQualifier requiredData;
        [SerializeField] 
        private List<GameUpgrade> requiredUpgrades;
        
        public string GetName() => name;
        public string Description => description;
        public bool IsProgressUpdate => isProgressUpdate;
        public CurveHandler ProgressCurve => progressCurve;
        public DataQualifier RequiredData => requiredData;

        public override bool Equals(object other)
        {
            var otherUpgrade = (GameUpgrade)other;
            return otherUpgrade != null && otherUpgrade.GetName().Equals(name);
        }
    }
}