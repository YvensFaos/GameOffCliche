using System.Collections.Generic;
using GameUI.Buttons;
using Progression;
using UnityEngine;
using Utils;

namespace GameUI
{
    public class UpgradeUIController : MonoBehaviour
    {
        [SerializeField]
        private List<GameUpgrade> processUpgrades;
        [SerializeField]
        private UpgradeButton upgradeButtonPrefab;
        [SerializeField]
        private Transform processButtonParent;
        [SerializeField] 
        private List<UpgradeButton> processButtons;

        private void OnEnable()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            TransformUtils.ClearObjects(processButtonParent);
            processButtons = new List<UpgradeButton>();
            processUpgrades.ForEach(upgrade =>
            {
                if (GameManager.Instance.CurrentPlayerData.IsUpgradeMaxedOut(upgrade)) return;
                var button = Instantiate(upgradeButtonPrefab, processButtonParent);
                processButtons.Add(button);
                button.Initialize(upgrade);
            });
        }
    }
}