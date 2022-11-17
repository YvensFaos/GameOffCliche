using System;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Resource Upgrade", menuName = "Cliche/Resource Upgrade", order = 0)]
    public class GameResourceUpgrade : GameUpgrade
    {
        [SerializeField]
        private ResourceType type;
        [SerializeField]
        private float increaseBy;
        
        public override void UpgradeUnlock(int level)
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            switch (type)
            {
                case ResourceType.HardDrive:
                    playerData.HardDriveSize += (int)increaseBy;
                    break;
                case ResourceType.StunResistance:
                    playerData.StunResistance += increaseBy;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.UpgradeUnlock(level);
        }
    }
}