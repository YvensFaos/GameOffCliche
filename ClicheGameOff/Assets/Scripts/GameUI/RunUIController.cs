using System.Collections.Generic;
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
        [SerializeField] private RectTransform hardDriveRectTransform;
        [SerializeField] private List<Button> startRunButtons;
        [SerializeField] private GameObject finishRunPanel;
        [SerializeField] private GameObject helperText;
        [SerializeField] private TextMeshProUGUI finishRunTextResults;
        [SerializeField] private Animator animator;
        [SerializeField] private List<GameObject> disableOnStartRun;

        private DataMinerRunController dataMinerRunController;
        private List<RectTransform> buttonsRectTransform;
        private TweenerCore<float, float, FloatOptions> hardDriveFillerTween;
        private static readonly int ShowResult = Animator.StringToHash("ShowResult");
        private static readonly int HideResult = Animator.StringToHash("HideResult");
        private static readonly int Step = Shader.PropertyToID("Step");

        private void Awake()
        {
            buttonsRectTransform = new List<RectTransform>();
            startRunButtons.ForEach(button => buttonsRectTransform.Add(button.GetComponent<RectTransform>()));
        }
        
        public void StartRun(DataSpawner spawner)
        {
            ToggleStartButtons(false);
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
            
            disableOnStartRun.ForEach(gameObjectToBeDisabled => gameObjectToBeDisabled.SetActive(false));
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
            hardDriveRectTransform.DOShakePosition(1.0f, 5.0f, 20);
        }

        private void FinishUIRun()
        {
            ToggleStartButtons(true);
            ToggleResultPanel(true);
            helperText.SetActive(false);
            dataMinerRunController.RemoveTickEvent(RunUITick);
            dataMinerRunController.RemoveFinishEvent(FinishUIRun);
            dataMinerRunController.RemoveCollectDataEvent(CollectData);

            var results = dataMinerRunController.GetResults();
            hardDriveFillSprite.DOFillAmount(0.0f, 1.0f);
            hardDriveRectTransform.DOShakePosition(1.0f, 10.0f);
            timeFillSprite.fillAmount = 0.0f;
            timeFillSprite.material.SetFloat(Step, 0.0f);
            finishRunTextResults.text = results;
            
            disableOnStartRun.ForEach(gameObjectToBeDisabled => gameObjectToBeDisabled.SetActive(true));
        }

        private void ToggleStartButtons(bool toggle)
        {
            for (var i = 0; i < startRunButtons.Capacity; i++)
            {
                var button = startRunButtons[i];
                var rect = buttonsRectTransform[i];
                if (toggle)
                {
                    button.gameObject.SetActive(true);
                    rect.DOAnchorPosX(20.0f, 0.75f).OnComplete(() =>
                    {
                        button.interactable = true;    
                    });
                }
                else
                {
                    button.interactable = false;
                    rect.DOAnchorPosX(-180.0f, 1.25f).OnComplete(() =>
                    {
                        button.gameObject.SetActive(false);    
                    });
                }
            }
        } 

        private void ToggleResultPanel(bool toggle)
        {
            animator.SetTrigger(toggle ? ShowResult : HideResult);
        }

    }
}
