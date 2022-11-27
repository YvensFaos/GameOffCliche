using System;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Resource Upgrade", menuName = "Cliche/Player Upgrade", order = 0)]
    public class GamePlayerStatsUpgrade : GameUpgrade
    {
        [SerializeField] private PlayerStatsType playerStatsType;
        [SerializeField] private float increaseBy;
        
        public override void UpgradeUnlock(int level)
        {
            PlayerData playerData = GameManager.Instance.CurrentPlayerData;
            switch (playerStatsType)
            {
                case PlayerStatsType.PlayerRadius:
                    playerData.PlayerRadius += increaseBy;
                    break;
                case PlayerStatsType.MiningRate:
                    playerData.MiningRate += increaseBy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.UpgradeUnlock(level);
        }
    }
}