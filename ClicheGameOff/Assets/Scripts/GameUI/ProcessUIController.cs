using System.Collections.Generic;
using GameUI.Buttons;
using Progression;
using UnityEngine;
using Utils;

namespace GameUI
{
    public class ProcessUIController : MonoBehaviour
    {
        [SerializeField]
        private List<GameUpgrade> processUpgrades;
        [SerializeField]
        private ProcessButton processButtonPrefab;
        [SerializeField]
        private Transform processButtonParent;
        [SerializeField] 
        private List<ProcessButton> processButtons;

        private void OnEnable()
        {
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            TransformUtils.ClearObjects(processButtonParent);
            processButtons = new List<ProcessButton>();
            processUpgrades.ForEach(upgrade =>
            {
                var button = Instantiate(processButtonPrefab, processButtonParent);
                processButtons.Add(button);
                button.Initialize(upgrade);
            });
        }
    }
}