using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RunUIController : MonoBehaviour
    {
        [SerializeField] private Image timeFillSprite;
        [SerializeField] private Button startRunButton;
        [SerializeField] private GameObject finishRunPanel;
        [SerializeField] private TextMeshProUGUI finishRunTextResults;
        
        private DataMinerRunController dataMinerRunController;
        
        public void StartRun()
        {
            ToggleStartButton(false);
            ToggleResultPanel(false);
            dataMinerRunController = GameManager.Instance.CurrentRun;
            timeFillSprite.fillAmount = 0.0f;
            dataMinerRunController.AddTickEvent(RunUITick);
            dataMinerRunController.StartRun();
            dataMinerRunController.AddFinishEvent(FinishUIRun);
        }

        private void RunUITick(float currentTime, float normalizedTime)
        {
            timeFillSprite.fillAmount = normalizedTime;
        }

        private void FinishUIRun()
        {
            ToggleStartButton(true);
            ToggleResultPanel(true);
            dataMinerRunController.RemoveTickEvent(RunUITick);
            dataMinerRunController.RemoveFinishEvent(FinishUIRun);

            var results = dataMinerRunController.GetResults();
            finishRunTextResults.text = results;
        }
        
        private void ToggleStartButton(bool toggle) => startRunButton.gameObject.SetActive(toggle);
        private void ToggleResultPanel(bool toggle) => finishRunPanel.gameObject.SetActive(toggle);
    }
}
