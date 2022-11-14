using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameUI.Buttons;
using Progression;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class PublishUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image publicationFillImage;
        [SerializeField] 
        private List<PublishUseDataButton> buttons;

        [Header("Progress")] 
        [SerializeField] private List<GamePublication> publications;

        private GamePublication currentPublication;
        private int publicationProgress;
        private int publicationNecessaryAmount;
        private int publicationCurrentAmount;
        private int goodDataUsedSoFar;
        private int badDataUsedSoFar;
        private bool publicationContentLimitReached;
        private TweenerCore<float, float, FloatOptions> fillTween;

        private void Start()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;
            publicationProgress = playerData.PublicationProgress;
            currentPublication = publications[publicationProgress];
            goodDataUsedSoFar = playerData.GoodDataUsedSoFar;
            badDataUsedSoFar = playerData.BadDataUsedSoFar;
            publicationNecessaryAmount = currentPublication.requiredContentAmount;
            publicationCurrentAmount = goodDataUsedSoFar + badDataUsedSoFar;
            
            buttons.ForEach(btn =>
            {
                btn.CurrentCost = currentPublication.contentCostPerTick;
                btn.UpdateValues();
            });
            
            NormalizeAndDisplayPublicationFill();
            UpdateUIController();
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

        private void NormalizeAndDisplayPublicationFill()
        {
            var normalized = Mathf.Clamp01(publicationCurrentAmount / (float) publicationNecessaryAmount);
            if (fillTween != null && fillTween.IsActive())
            {
                fillTween.Kill();
            }
            fillTween = publicationFillImage.DOFillAmount(normalized, 0.2f);
        }

        private void UpdateUIController()
        {
            publicationContentLimitReached = publicationCurrentAmount >= publicationNecessaryAmount;
            buttons.ForEach(btn => btn.ToggleInteractivity(!publicationContentLimitReached));
        }
    }
}
