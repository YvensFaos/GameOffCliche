using System;
using UnityEngine.Events;
using Utils;

namespace Progression
{
    [Serializable]
    public class GameUpgradeEventPair : Pair<GameUpgrade, UnityEvent<PlayerData>>
    {
        public GameUpgradeEventPair(GameUpgrade one, UnityEvent<PlayerData> two) : base(one, two)
        { }

        public void UnlockUpgrade(PlayerData playerData)
        {
            Two?.Invoke(playerData);
        }
    }
}