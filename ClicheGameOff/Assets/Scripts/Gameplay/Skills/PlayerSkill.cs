using UnityEngine;

namespace Gameplay.Skills
{
    public abstract class PlayerSkill : MonoBehaviour
    {
        [SerializeField] private GameSkill skill;

        protected void Start()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.SubscribeUseSkillDelegate(UseSkill);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
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