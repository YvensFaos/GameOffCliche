using System;
using UnityEngine;
using Utils;

namespace Progression
{
    [Serializable]
    public class GameUpgradeLevelPair : Pair<GameUpgrade, int> {
        public GameUpgradeLevelPair(GameUpgrade one, int two) : base(one, two)
        { }

        public bool IncreaseLevel()
        {
            Two = (int) Mathf.Min(Two + 1, One.ProgressCurve.GetMaxLevel());
            return Two >= One.ProgressCurve.GetMaxLevel();
        }
        
        public void SetLevel(int level)
        {
            Two = (int) Mathf.Min(level, One.ProgressCurve.GetMaxLevel());
        } 

        public int GetLevel() => Two;
    }
}