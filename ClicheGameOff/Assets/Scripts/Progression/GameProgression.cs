using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "Game Progress", menuName = "Cliche/Game Progress", order = 0)]
    public class GameProgression : ScriptableObject
    {
        [SerializeField] private List<GameUpgrade> gameUpgrades;

        public GameUpgrade GetUpgrade(string upgradeName)
        {
            return gameUpgrades.Find(upgrade => upgrade.GetName().Equals(upgradeName));
        }
    }
}