using System.Collections.Generic;
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
    }
}