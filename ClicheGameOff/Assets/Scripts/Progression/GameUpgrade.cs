using System.Collections.Generic;
using System.Linq;
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
        private CurveHandler valueCurve;
        [SerializeField] 
        private DataQualifier requiredData;
        [SerializeField] 
        protected List<GameUpgrade> requiredUpgrades;
        
        public string GetName() => name;
        public string Description => description;
        public bool RepeatableUpgrade => repeatableUpgrade;
        public CurveHandler ProgressCurve => progressCurve;
        public CurveHandler ValueCurve => valueCurve;
        public DataQualifier RequiredData => requiredData;

        public virtual void UpgradeUnlock(int level)
        {
            GameManager.Instance.UpgradeUnlock(this, level);
        }

        public virtual bool CheckRequirements()
        {
            if (!HasRequirements()) return true;
            var playerData = GameManager.Instance.CurrentPlayerData;
            return requiredUpgrades.Select(upgrade => playerData.GetUpgradeLevel(upgrade)).All(level => level > -1);
        }

        public bool HasRequirements() => requiredUpgrades.Count > 0;

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