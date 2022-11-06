using System;
using Progression;
using UnityEngine;
using Utils;

namespace Data
{
    [Serializable]
    public class GameUpgradeMaterialPair : Pair<GameUpgrade, Material>
    {
        public GameUpgradeMaterialPair(GameUpgrade one, Material two) : base(one, two)
        { }
    }
}