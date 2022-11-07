using System;
using Progression;
using Utils;

[Serializable]
public class GameUpgradeNameLevelPair : Pair<string, int>
{
    public GameUpgradeNameLevelPair(string one, int two) : base(one, two)
    { }

    public GameUpgradeLevelPair ToGameUpgradeLevelPair()
    {
        return new GameUpgradeLevelPair(GameManager.Instance.GetUpgradeByName(One), Two);
    }
}