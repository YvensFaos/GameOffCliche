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
        [SerializeField] private GameObject helperText;
        [SerializeField] private TextMeshProUGUI finishRunTextResults;
        [SerializeField] private Animator animator;

        private DataMinerRunController dataMinerRunController;
        
        private TweenerCore<float, float, FloatOptions> hardDriveFillerTween;
        private static readonly int ShowResult = Animator.StringToHash("ShowResult");
        private static readonly int HideResult = Animator.StringToHash("HideResult");
        private static readonly int Step = Shader.PropertyToID("Step");

        public void StartRun(DataSpawner spawner)
        {
            ToggleStartButton(false);
            finishRunPanel.SetActive(false);
            helperText.SetActive(true);
            dataMinerRunController = GameManager.Instance.MainRunner;
            timeFillSprite.fillAmount = 0.0f;
            
            timeFillSprite.material.SetFloat(Step, 1.0f - timeFillSprite.fillAmount);
            hardDriveFillSprite.fillAmount = 0.0f;
            
            dataMinerRunController.AddTickEvent(RunUITick);
            dataMinerRunController.StartRun(spawner);
            dataMinerRunController.AddFinishEvent(FinishUIRun);
            dataMinerRunController.AddCollectDataEvent(CollectData);
        }

        private void RunUITick(float currentTime, float normalizedTime)
        {
            timeFillSprite.fillAmount = normalizedTime;
            timeFillSprite.material.SetFloat(Step, 1.0f - timeFillSprite.fillAmount);
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
            helperText.SetActive(false);
            dataMinerRunController.RemoveTickEvent(RunUITick);
            dataMinerRunController.RemoveFinishEvent(FinishUIRun);
            dataMinerRunController.RemoveCollectDataEvent(CollectData);

            var results = dataMinerRunController.GetResults();
            hardDriveFillSprite.DOFillAmount(0.0f, 1.0f);
            timeFillSprite.fillAmount = 0.0f;
            timeFillSprite.material.SetFloat(Step, 0.0f);
            finishRunTextResults.text = results;
        }
        
        private void ToggleStartButton(bool toggle) => startRunButton.gameObject.SetActive(toggle);

        private void ToggleResultPanel(bool toggle)
        {
            animator.SetTrigger(toggle ? ShowResult : HideResult);
        }

    }
}
