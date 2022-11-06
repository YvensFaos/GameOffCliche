using System.Collections.Generic;
using Gameplay.Skills;
using GameUI.Buttons;
using UnityEngine;
using Utils;

namespace GameUI
{
    public class PlayerSkillsUIController : MonoBehaviour
    {
        [SerializeField]
        private List<GameSkill> skills;
        [SerializeField]
        private SkillButton skillButtonPrefab;
        [SerializeField]
        private Transform skillButtonParent;
        [SerializeField] 
        private List<SkillButton> skillButtons;
        
        private void Start()
        {
            GenerateButtons();
            GameManager.Instance.SubscribeUpdatePlayerInfo(UpdatePlayerInfo);
        }

        private void OnDestroy()
        {
            GameManager.Instance.UnsubscribeUpdatePlayerInfo(UpdatePlayerInfo);
        }

        private void GenerateButtons()
        {
            TransformUtils.ClearObjects(skillButtonParent);
            skillButtons = new List<SkillButton>();
            skills = GameManager.Instance.CurrentPlayerData.Skills;
            skills?.ForEach(upgrade =>
            {
                var button = Instantiate(skillButtonPrefab, skillButtonParent);
                skillButtons.Add(button);
                button.Initialize(upgrade);
            });
        }

        private void UpdatePlayerInfo(in PlayerData playerData)
        {
            GenerateButtons();
        }
    }
}