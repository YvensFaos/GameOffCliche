using System;
using UnityEngine.Events;
using Utils;

namespace Gameplay.Skills
{
    [Serializable]
    public class GameSkillEventPair : Pair<GameSkill, UnityEvent<PlayerController>>
    {
        public GameSkillEventPair(GameSkill one, UnityEvent<PlayerController> two) : base(one, two)
        { }
    }
}