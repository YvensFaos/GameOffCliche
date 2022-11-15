using System.Collections.Generic;
using Dialog;
using Gameplay;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Game Publication", menuName = "Cliche/Game Publication", order = 0)]
    public class GamePublication : ScriptableObject
    {
        public int requiredContentAmount;
        public int contentCostPerTick;
        public DataSpawnerList spawnerUnlocked;
        public List<GameUpgrade> upgradesToOpen;
        public List<GameUpgrade> upgradesToUnlock;
        public GameDialog dialog;

        public string UpgradesText()
        {
            var unlockedText = "";
            upgradesToOpen.ForEach(upgrade =>
            {
                unlockedText += $"Unlocked: <b>{upgrade.name}</b>!\r\n";
            });

            upgradesToUnlock.ForEach(upgrade =>
            {
                unlockedText += $"Discovered:  <b>{upgrade.name}</b>!\r\n";
            });

            return unlockedText;
        }
    }
}