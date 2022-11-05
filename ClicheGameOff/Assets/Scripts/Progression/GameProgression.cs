using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "Game GameProgress", menuName = "Cliche/Game GameProgress", order = 0)]
    public class GameProgression : ScriptableObject
    {
        public AnimationCurve betterResearchSkillsCurve;
        public AnimationCurve betterFactCheckingCurve;
    }
}