using System;

namespace Progression
{
    public class GameUpgradeNotFoundException : Exception
    {
        public GameUpgradeNotFoundException(GameUpgrade upgrade) : base( $"Game Upgrade not found. Upgrade: {upgrade.GetName()}.")
        { }
    }
}