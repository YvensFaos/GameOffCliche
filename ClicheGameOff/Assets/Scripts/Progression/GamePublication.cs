using System.Collections.Generic;
using Dialog;
using Gameplay;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Game Publication", menuName = "Cliche/Game Publication", order = 0)]
    public class GamePublication : ScriptableObject, IComparer<GamePublication>
    {
        public int order;
        public int requiredContentAmount;
        public int contentCostPerTick;
        public DataSpawnerList spawnerUnlocked;
        public List<GameUpgradeLevelPair> upgradesToOpen;
        public List<GameUpgrade> upgradesToUnlock;
        public GameDialog dialog;

        public void UnlockPublication()
        {
            if (spawnerUnlocked != null)
            {
                Debug.Log($"Change spawner to -> {spawnerUnlocked.name}");
                GameManager.Instance.MainSpawner.SetDataSpawnerList(spawnerUnlocked);
            }
            upgradesToOpen.ForEach(upgradeLevelPair =>
            {
                Debug.Log($"Upgrade -> {upgradeLevelPair.One.name}");
                upgradeLevelPair.UpgradeUnlock();
            });
            upgradesToUnlock.ForEach(upgrade =>
            {
                Debug.Log($"Open Upgrade -> {upgrade.name}");
                GameManager.Instance.OpenUpgrade(upgrade);
            });
        }
        
        public string UpgradesText()
        {
            var unlockedText = "";
            upgradesToOpen.ForEach(upgrade =>
            {
                unlockedText += $"Unlocked: <b>{upgrade.One.name}</b>!\r\n";
            });

            upgradesToUnlock.ForEach(upgrade =>
            {
                unlockedText += $"Discovered:  <b>{upgrade.name}</b>!\r\n";
            });

            return unlockedText;
        }

        public int Compare(GamePublication x, GamePublication y)
        {
            if (x == null) return -1;
            if (y != null)
                return x.order.CompareTo(y.order);
            return -1;
        }
    }
}