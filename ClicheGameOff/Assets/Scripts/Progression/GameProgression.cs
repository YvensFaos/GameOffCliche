using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "Game Progress", menuName = "Cliche/Game Progress", order = 0)]
    public class GameProgression : ScriptableObject
    {
        [SerializeField] private List<GameUpgradeLevelPair> gameUpgrades;

        public int GetGameUpgradeLevel(GameUpgrade upgrade)
        {
            var upgradePair = gameUpgrades.Find(pair => pair.One.Equals(upgrade));
            return upgradePair?.GetLevel() ?? -1;
        }

        public bool IncreaseGameUpgradeLevel(GameUpgrade upgrade)
        {
            var upgradePair = gameUpgrades.Find(pair => pair.One.Equals(upgrade));
            if (upgradePair != null)
            {
                return upgradePair.IncreaseLevel();
            }
            else
            {
                throw new GameUpgradeNotFoundException(upgrade);
            }
        }
        
        public void SetGameUpgradeLevel(GameUpgrade upgrade, int level)
        {
            var upgradePair = gameUpgrades.Find(pair => pair.One.Equals(upgrade));
            if (upgradePair != null)
            {
                upgradePair.SetLevel(level);
            }
            else
            {
                throw new GameUpgradeNotFoundException(upgrade);
            }
        }
    }
}