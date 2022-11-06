using System;
using Data;

namespace Progression
{
    public class GameUpgradeNotFoundException : Exception
    {
        private GameUpgrade upgrade;

        public GameUpgradeNotFoundException(GameUpgrade upgrade) : base( $"Game Upgrade not found. Upgrade: {upgrade.GetName()}.")
        {
            Upgrade = upgrade;
        }

        public GameUpgrade Upgrade
        {
            get => upgrade;
            set => upgrade = value;
        }
    }
}