using System.Collections.Generic;
using Data;
using UnityEngine;
using Utils;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Game Upgrade", menuName = "Cliche/Game Upgrade", order = 0)]
    public class GameUpgrade : ScriptableObject
    {
        [SerializeField, TextArea]
        private string description;
        [SerializeField]
        private bool repeatableUpgrade;
        [SerializeField] 
        private CurveHandler progressCurve;
        [SerializeField] 
        private DataQualifier requiredData;
        [SerializeField] 
        private List<GameUpgrade> requiredUpgrades;
        
        public string GetName() => name;
        public string Description => description;
        public bool RepeatableUpgrade => repeatableUpgrade;
        public CurveHandler ProgressCurve => progressCurve;
        public DataQualifier RequiredData => requiredData;

        public virtual void UpgradeUnlock()
        {
            GameManager.Instance.UpgradeUnlocked(this);
        }

        public override bool Equals(object other)
        {
            var otherUpgrade = (GameUpgrade)other;
            return otherUpgrade != null && otherUpgrade.GetName().Equals(name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}