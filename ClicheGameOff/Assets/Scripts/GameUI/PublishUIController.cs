using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameUI.Buttons;
using Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace GameUI
{
    public class PublishUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image publicationFillImage;
        [SerializeField] 
        private List<PublishUseDataButton> buttons;
        [SerializeField] 
        private TextMeshProUGUI progressText;
        [SerializeField] 
        private SuccessPublicationController successPanel;
        [SerializeField] 
        private GameObject failurePanel;

        [Header("Progress")] 
        [SerializeField] private List<GamePublication> publications;

        private GamePublication currentPublication;
        private int publicationProgress;
        private int publicationNecessaryAmount;
        private int publicationCurrentAmount;
        private int goodDataUsedSoFar;
        private int badDataUsedSoFar;
        private float normalizedPublicationProgress;
        private float publicationChance;
        private bool publicationContentLimitReached;
        private TweenerCore<float, float, FloatOptions> fillTween;

        private void Start()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            publicationProgress = playerData.PublicationProgress;
            UpdateCurrentPublication();
            goodDataUsedSoFar = playerData.GoodDataUsedSoFar;
            badDataUsedSoFar = playerData.BadDataUsedSoFar;
            publicationCurrentAmount = goodDataUsedSoFar + badDataUsedSoFar;
            
            UpdateButtonCosts();
            NormalizeAndDisplayPublicationFill();
            UpdateUIController();
        }

        private void UpdateCurrentPublication()
        {
            if (publicationProgress < publications.Count)
            {
                currentPublication = publications[publicationProgress];
                publicationNecessaryAmount = currentPublication.requiredContentAmount;
            }
            else
            {
                currentPublication = publications[^1];
                //Max publication reached!
            }
        }

        private void UpdateButtonCosts()
        {
            buttons.ForEach(btn =>
            {
                btn.CurrentCost = currentPublication.contentCostPerTick;
                btn.UpdateValues();
            });
        }

        public void IncrementPublicationContent(DataQualifier incrementType, int amount)
        {
            publicationCurrentAmount += amount;
            //If the player, for some reason, added more data than necessary,
            //Then calculate the left over to give it back
            var leftOver = Mathf.Max(0, publicationCurrentAmount - publicationNecessaryAmount);
            //Amount used wil be clamped
            amount -= leftOver;

            //Clamp the current amount to keep it within its limits
            publicationCurrentAmount = Mathf.Clamp(publicationCurrentAmount, 0, publicationNecessaryAmount);
            
            switch (incrementType)
            {
                case DataQualifier.Good:
                    goodDataUsedSoFar += amount;
                    GameManager.Instance.ManagePlayerCollected(incrementType, -amount);
                    
                    break;
                case DataQualifier.Bad:
                    badDataUsedSoFar += amount;
                    GameManager.Instance.ManagePlayerCollected(incrementType, -amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(incrementType), incrementType, null);
            }
            
            NormalizeAndDisplayPublicationFill();
            UpdateUIController();
        }

        public void ResetPublication()
        {
            GameManager.Instance.ManagePlayerCollected(DataQualifier.Good, goodDataUsedSoFar);
            goodDataUsedSoFar = 0;
            GameManager.Instance.ManagePlayerCollected(DataQualifier.Bad, badDataUsedSoFar);
            badDataUsedSoFar = 0;
            publicationCurrentAmount = 0;
            
            NormalizeAndDisplayPublicationFill();
            UpdateUIController();
        }

        public void SubmitPublication()
        {
            NormalizeAndDisplayPublicationFill();
            
            if (RandomChanceUtils.GetChance(publicationChance * 100.0f))
            {
                //Success!
                var paper = GameManager.Instance.PublishNewPaper(currentPublication, goodDataUsedSoFar, badDataUsedSoFar);
                successPanel.gameObject.SetActive(true);
                successPanel.Initialize(paper, currentPublication);
                
                var playerData = GameManager.Instance.CurrentPlayerData;
                playerData.PublicationProgress++;
                
                currentPublication.UnlockPublication();
                UpdateCurrentPublication();
                UpdateButtonCosts();
            }
            else
            {
                //Fail
                failurePanel.gameObject.SetActive(true);
            }

            //Reset data used so far - it is lost.
            goodDataUsedSoFar = 0;
            badDataUsedSoFar = 0;
            ResetPublication();
        }

        private void NormalizeAndDisplayPublicationFill()
        {
            normalizedPublicationProgress = Mathf.Clamp01(publicationCurrentAmount / (float) publicationNecessaryAmount);
            publicationChance = Mathf.Clamp01(goodDataUsedSoFar / (float) publicationNecessaryAmount);
            if (fillTween != null && fillTween.IsActive())
            {
                fillTween.Kill();
            }
            fillTween = publicationFillImage.DOFillAmount(normalizedPublicationProgress, 0.2f);
        }

        private void UpdateUIController()
        {
            publicationContentLimitReached = publicationCurrentAmount >= publicationNecessaryAmount;
            buttons.ForEach(btn => btn.ToggleInteractivity(!publicationContentLimitReached));

            if (publicationChance <= 0)
            {
                progressText.text = "Nothing written at all.. Better to get started soon!";
            }
            else
            {
                var result = "";
                var successRatio = publicationChance * normalizedPublicationProgress;
                result = successRatio switch
                {
                    <= 0.1f => "What? This paper is just a collection of memes!",
                    <= 0.25f => "Where are the footnotes? This is a mess!",
                    <= 0.5f => "Half of this is complete garbage. Let's start over!",
                    <= 0.75f => "Some of those results are truly biased and skewed!",
                    <= 0.9f => "It looks overall good, but we can use more proper research.",
                    <= 1.0f => "Not bad. This paper has some good chances of being accepted.!",
                    _ => result
                };
                result += $" [{Mathf.CeilToInt(successRatio * 100)}% of Success]";
                progressText.text = result;
            }
        }
    }
}
