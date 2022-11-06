using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Game Skill Upgrade", menuName = "Cliche/Game Skill Upgrade", order = 0)]
    public class GameSkillUpgrade : GameUpgrade
    {
        public override void UpgradeUnlock(int level)
        {
            base.UpgradeUnlock(level);
            Debug.Log("Unlock skill");
        }
    }
}