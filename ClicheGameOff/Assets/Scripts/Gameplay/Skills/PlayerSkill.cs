using UnityEngine;

namespace Gameplay.Skills
{
    public abstract class PlayerSkill : MonoBehaviour
    {
        [SerializeField] private GameSkill skill;

        protected void Start()
        {
            GameManager.Instance.SubscribeUseSkillDelegate(UseSkill);
        }

        private void OnDestroy()
        {
            GameManager.Instance.UnsubscribeUseSkillDelegate(UseSkill);
        }

        private void UseSkill(GameSkill usedSkill, in PlayerController playerController)
        {
            if (skill.Equals(usedSkill))
            {
                SkillEffect(playerController);
            }
        }

        protected abstract void SkillEffect(in PlayerController playerController);
    }
}