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
        [SerializeField] 
        protected GameUpgradeType upgradeType;
        [SerializeField, TextArea]
        private string description;
        [SerializeField]
        private bool repeatableUpgrade;
        [SerializeField] 
        private CurveHandler progressCurve;
        [SerializeField] 
        private CurveHandler valueCurve;
        [SerializeField] 
        private int uniqueCost;
        [SerializeField] 
        private DataQualifier requiredData;
        [SerializeField] 
        protected List<GameUpgrade> requiredUpgrades;
        [SerializeField] 
        protected List<GamePublication> requiredPublications;

        public virtual void UpgradeUnlock(int level)
        {
            GameManager.Instance.UpgradeUnlock(this, level);
        }

        public bool CheckRequirements()
        {
            if (!HasRequirements()) return true;
            var playerData = GameManager.Instance.CurrentPlayerData;
            var hasUpgrades = requiredUpgrades.Select(upgrade => playerData.GetUpgradeLevel(upgrade)).All(level => level > -1);
            var hasPublications = requiredPublications.Select(publication =>
            {
                return playerData.Papers.FindIndex(paper => paper.publicationName.Equals(publication.name));
            }).All(index => index >= 0);

            return hasUpgrades && hasPublications;
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

        public override string ToString()
        {
            return name;
        }

        //Getters & Setters
        public string GetName() => name;
        public string Description => description;
        public bool RepeatableUpgrade => repeatableUpgrade;
        public CurveHandler ProgressCurve => progressCurve;
        public CurveHandler ValueCurve => valueCurve;
        public DataQualifier RequiredData => requiredData;
        public int UniqueCost => uniqueCost;
        protected List<GamePublication> RequiredPublications => requiredPublications;
        public GameUpgradeType UpgradeType => upgradeType;
    }
}