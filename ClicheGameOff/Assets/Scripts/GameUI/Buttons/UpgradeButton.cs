using Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Buttons
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private GameUpgrade processUpgrade;
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private TextMeshProUGUI resourceNeeded;
        [SerializeField] private Image resourceImage;
        [SerializeField] private Button processButton;

        private int currentCost = -1;
        private bool maxLevel;
        
        public void Initialize(GameUpgrade upgrade)
        {
            processUpgrade = upgrade;
        } 
        
        private void Start()
        {
            upgradeName.text = processUpgrade.GetName();
            upgradeDescription.text = processUpgrade.Description;
            resourceImage.color = GameManager.Instance.Constants.GetColorForQualifier(processUpgrade.RequiredData);
            
            UpdateLabels();
            if (!maxLevel) return;
            DisableButton();
        }

        private void UpdateLabels()
        {
            var currentLevel = GameManager.Instance.GameProgress.GetGameUpgradeLevel(processUpgrade);
            currentCost = (int)processUpgrade.ProgressCurve.EvaluateAtLevel(currentLevel, out maxLevel);
            resourceNeeded.text = currentCost.ToString();
        }

        private void DisableButton()
        {
            processButton.interactable = false;
            resourceNeeded.gameObject.SetActive(false);
        }

        public void Click()
        {
            if (!maxLevel && GameManager.Instance.CheckPlayerData(processUpgrade.RequiredData, currentCost))
            {
                //Notice that the current cost is given as a negative value
                GameManager.Instance.ManagePlayerCollected(processUpgrade.RequiredData, -currentCost);
                maxLevel = GameManager.Instance.GameProgress.IncreaseGameUpgradeLevel(processUpgrade);
                var currentLevel = GameManager.Instance.GameProgress.GetGameUpgradeLevel(processUpgrade);
                processUpgrade.UpgradeUnlock(currentLevel);
                if (maxLevel || !processUpgrade.RepeatableUpgrade)
                {
                    DisableButton();
                }
                if (processUpgrade.RepeatableUpgrade)
                {
                    UpdateLabels();
                }
            }
            else
            {
                //Buzz sound!
            }
        }
    }
}