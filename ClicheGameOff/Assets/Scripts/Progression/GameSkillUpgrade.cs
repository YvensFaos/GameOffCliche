using System.Linq;
using Gameplay.Skills;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "New Game Skill Upgrade", menuName = "Cliche/Game Skill Upgrade", order = 0)]
    public class GameSkillUpgrade : GameUpgrade
    {
        [SerializeField]
        private GameSkill skill;

        public override void UpgradeUnlock(int level)
        {
            base.UpgradeUnlock(level);
            GameManager.Instance.CurrentPlayerData.AddSkill(skill);
        }

        /// <summary>
        /// Skill Upgrades requires ANY of its requirements to be available. 
        /// </summary>
        /// <returns></returns>
        public override bool CheckRequirements()
        {
            if (!HasRequirements()) return true;
            
            var playerData = GameManager.Instance.CurrentPlayerData;
            return requiredUpgrades.Select(upgrade => playerData.GetUpgradeLevel(upgrade)).Any(level => level > -1);
        }
    }
}