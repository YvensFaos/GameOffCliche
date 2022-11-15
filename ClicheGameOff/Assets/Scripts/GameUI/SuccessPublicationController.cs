using Dialog;
using Progression;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class SuccessPublicationController : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI paperTitle;
        [SerializeField] 
        private TextMeshProUGUI unlockedArea;
        [SerializeField] 
        private GameObject publicationPanel;

        private GameDialog dialogToPayWhenClose; 
        
        public void Initialize(PublishedPaper paper, GamePublication publication)
        {
            paperTitle.text = paper.paperTitle;
            unlockedArea.text = publication.UpgradesText();
            dialogToPayWhenClose = publication.dialog;
        }

        public void Close()
        {
            if (dialogToPayWhenClose == null) return;
            publicationPanel.SetActive(false);
            GameManager.Instance.DialogController.StartDialog(dialogToPayWhenClose);
        }
    }
}