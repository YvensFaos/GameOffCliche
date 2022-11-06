using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    public class GameUpgradeUnlocker : MonoBehaviour
    {
        [SerializeField]
        private List<GameUpgradeEventPair> unlockEvents;

        public void UnlockEvent(GameUpgrade upgrade, PlayerData playerData)
        {
            var unlockPair = unlockEvents.Find(pair => pair.One.Equals(upgrade));
            unlockPair?.UnlockUpgrade(playerData);
        }
    }
}