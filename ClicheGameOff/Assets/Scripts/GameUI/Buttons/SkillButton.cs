using DG.Tweening;
using Gameplay;
using Gameplay.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Buttons
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private GameSkill skill;
        [SerializeField] private TextMeshProUGUI shortCut;
        [SerializeField] private TextMeshProUGUI skillTitle;
        [SerializeField] private Image skillImage;
        [SerializeField] private Button skillButton;

        private bool isOnCoolDown;
        private Tween cooldownFillTween;
        
        public void Initialize(GameSkill gameSkill)
        {
            skill = gameSkill;
            GameManager.Instance.SubscribeUseSkillDelegate(UseSkillDelegate);
        }

        public void OnDestroy()
        {
            GameManager.Instance.UnsubscribeUseSkillDelegate(UseSkillDelegate);
        }

        private void Start()
        {
            shortCut.text = skill.shortCut.ToString().ToUpper();
            skillTitle.text = skill.GetName();
            skillImage.fillAmount = 1;
        }

        private void UseSkillDelegate(GameSkill usedSkill, in PlayerController playerController)
        {
            if (skill.Equals(usedSkill))
            {
                //Execute the button animation, but ignores the call to actually execute the skill.
                AnimateButtonCooldown();
            }
        }

        public void Click()
        {
            GameManager.Instance.Player.TryToUseSkill(skill);
        }

        private void AnimateButtonCooldown()
        {
            if (isOnCoolDown) return;
            if (cooldownFillTween != null && cooldownFillTween.IsActive())
            {
                cooldownFillTween.Kill();
            }

            isOnCoolDown = true;
            skillButton.interactable = false;
            skillImage.fillAmount = 0.0f;
            cooldownFillTween = skillImage.DOFillAmount(1.0f, skill.cooldown).OnComplete(() =>
            {
                isOnCoolDown = false;
                skillButton.interactable = true;
            });
        }
    }
}
