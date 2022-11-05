using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RunUIController : MonoBehaviour
    {
        [SerializeField] private Image timeFillSprite;
        [SerializeField] private Image hardDriveFillSprite;
        [SerializeField] private Button startRunButton;
        [SerializeField] private GameObject finishRunPanel;
        [SerializeField] private TextMeshProUGUI finishRunTextResults;
        
        private DataMinerRunController dataMinerRunController;
        
        private TweenerCore<float, float, FloatOptions> hardDriveFillerTween;

        public void StartRun()
        {
            ToggleStartButton(false);
            ToggleResultPanel(false);
            dataMinerRunController = GameManager.Instance.CurrentRun;
            timeFillSprite.fillAmount = 0.0f;
            hardDriveFillSprite.fillAmount = 0.0f;
            
            dataMinerRunController.AddTickEvent(RunUITick);
            dataMinerRunController.StartRun();
            dataMinerRunController.AddFinishEvent(FinishUIRun);
            dataMinerRunController.AddCollectDataEvent(CollectData);
        }

        private void RunUITick(float currentTime, float normalizedTime)
        {
            timeFillSprite.fillAmount = normalizedTime;
        }

        private void CollectData(BaseDataBehavior dataBehavior, float normalizedHardDriveUsed)
        {
            if (hardDriveFillerTween != null && hardDriveFillerTween.IsActive())
            {
                hardDriveFillerTween.Kill();
            }
            hardDriveFillerTween = hardDriveFillSprite.DOFillAmount(normalizedHardDriveUsed, 0.2f);
        }

        private void FinishUIRun()
        {
            ToggleStartButton(true);
            ToggleResultPanel(true);
            dataMinerRunController.RemoveTickEvent(RunUITick);
            dataMinerRunController.RemoveFinishEvent(FinishUIRun);
            dataMinerRunController.RemoveCollectDataEvent(CollectData);

            var results = dataMinerRunController.GetResults();

            hardDriveFillSprite.DOFillAmount(0.0f, 1.0f);
            finishRunTextResults.text = results;
        }
        
        private void ToggleStartButton(bool toggle) => startRunButton.gameObject.SetActive(toggle);
        private void ToggleResultPanel(bool toggle) => finishRunPanel.gameObject.SetActive(toggle);
    }
}
