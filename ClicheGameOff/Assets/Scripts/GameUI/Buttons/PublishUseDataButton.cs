using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI.Buttons
{
    public class PublishUseDataButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI buttonText;
        [SerializeField] 
        private Button publishButton;
        [SerializeField] 
        private PublishUIController controller;

        [SerializeField]
        private DataQualifier dataType;
        [SerializeField]
        private int currentCost;

        public void UpdateValues()
        {
            buttonText.text = $"{GetButtonText()} [{currentCost}]";
        }
        
        public void ClickMe()
        {
            if (GameManager.Instance.CheckPlayerData(dataType, currentCost))
            {
                controller.IncrementPublicationContent(dataType, currentCost);
            }
            else
            {
                //FAIL - not enough resources
            }
        }

        private string GetButtonText()
        {
            return dataType switch
            {
                DataQualifier.Good => "Good Data ",
                DataQualifier.Bad => "Bad Data ",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void ToggleInteractivity(bool toggle)
        {
            publishButton.interactable = toggle;
        }

        public int CurrentCost
        {
            set => currentCost = value;
        }
    }
}
