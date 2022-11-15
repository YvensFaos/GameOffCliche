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
            if (GameManager.Instance == null) return;
            GameManager.Instance.UnsubscribeUpdatePlayerInfo(UpdatePlayerInfo);
        }

        private void GenerateButtons()
        {
            TransformUtils.ClearObjects(skillButtonParent);
            skillButtons = new List<SkillButton>();
            skills = GameManager.Instance.CurrentPlayerData.Skills;
            skills?.ForEach(skill =>
            {
                if (!GameManager.Instance.CurrentPlayerData.HasSkill(skill)) return;
                var button = Instantiate(skillButtonPrefab, skillButtonParent);
                skillButtons.Add(button);
                button.Initialize(skill);
            });
        }

        private void UpdatePlayerInfo(in PlayerData playerData)
        {
            GenerateButtons();
        }
    }
}