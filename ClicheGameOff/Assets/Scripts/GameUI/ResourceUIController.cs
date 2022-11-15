using TMPro;
using UnityEngine;

namespace GameUI
{
    public class ResourceUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI goodDataText;
        [SerializeField]
        private TextMeshProUGUI badDataText;

        private void OnEnable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.SubscribeUpdatePlayerInfo(UpdatePlayerInfo);
            
            //Manually update the values when enabling this component
            UpdatePlayerInfo(GameManager.Instance.CurrentPlayerData);
        }

        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.UnsubscribeUpdatePlayerInfo(UpdatePlayerInfo);
        }

        private void UpdatePlayerInfo(in PlayerData playerData)
        {
            goodDataText.text = playerData.GoodData.ToString();
            badDataText.text = playerData.BadData.ToString();
        }
    }
}
