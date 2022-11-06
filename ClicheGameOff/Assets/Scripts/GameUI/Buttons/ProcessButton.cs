using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Buttons
{
    public class ProcessButton : MonoBehaviour
    {
        [SerializeField] private GameUpgrade processUpgrade;
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private TextMeshProUGUI resourceNeeded;
        [SerializeField] private Image resourceImage;
        [SerializeField] private Button processButton;

        private int currentCost = -1;
        private bool maxLevel;
        
        private void Start()
        {
            upgradeName.text = processUpgrade.GetName();
            upgradeDescription.text = processUpgrade.Description;
            resourceImage.color = GameManager.Instance.Constants.GetColorForQualifier(processUpgrade.RequiredData);
            resourceNeeded.text = "0";
        }
        
        private void OnEnable()
        {
            currentCost = (int) processUpgrade.ProgressCurve.EvaluateAtLevel(0, out maxLevel);
            if (!maxLevel) return;
            processButton.interactable = false;
            resourceNeeded.gameObject.SetActive(false);
        }

        private void Click()
        {
            if (!maxLevel && GameManager.Instance.CheckPlayerData(processUpgrade.RequiredData, currentCost))
            {
                GameManager.Instance.ManagePlayerCollected(processUpgrade.RequiredData, currentCost);
                
            }
            else
            {
                //Buzz sound!
            }
        }
    }
}