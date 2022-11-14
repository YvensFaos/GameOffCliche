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
        
        public void Initialize(PublishedPaper paper, GamePublication publication)
        {
            paperTitle.text = paper.paperTitle;
            unlockedArea.text = publication.UpgradesText();
        }
    }
}