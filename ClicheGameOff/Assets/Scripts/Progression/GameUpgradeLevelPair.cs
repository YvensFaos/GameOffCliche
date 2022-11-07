using System;
using UnityEngine;
using Utils;

namespace Progression
{
    [Serializable]
    public class GameUpgradeLevelPair : Pair<GameUpgrade, int> {
        public GameUpgradeLevelPair(GameUpgrade one, int two) : base(one, two)
        { }

        public void UpgradeUnlock()
        {
            One.UpgradeUnlock(Two);
        }
        
        public bool IncreaseLevel()
        {
            Two = (int) Mathf.Min(Two + 1, One.ProgressCurve.GetMaxLevel());
            return Two >= One.ProgressCurve.GetMaxLevel();
        }
        
        public bool SetLevel(int level)
        {
            Two = (int) Mathf.Min(level, One.ProgressCurve.GetMaxLevel());
            return Two >= One.ProgressCurve.GetMaxLevel();
        }

        public int GetLevel() => Two;
    }
}