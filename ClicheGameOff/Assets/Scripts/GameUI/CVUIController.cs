using TMPro;
using UnityEngine;

namespace GameUI
{
    public class CVUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI cvText;

        private void OnEnable()
        {
            var playerData = GameManager.Instance.CurrentPlayerData;

            var playerDataText = "";
            playerDataText += $"<b>Good Data:</b> {playerData.GoodData}\r\n";
            playerDataText += $"<b>Bad Data:</b> {playerData.BadData}\r\n";
            playerDataText += $"<b>Hard Drive:</b> {playerData.HardDriveSize}\r\n";
            playerDataText += $"<b>Radius:</b> {playerData.PlayerRadius}\r\n";
            playerDataText += $"<b>Mining Rate:</b> {playerData.MiningRate}\r\n";
            playerDataText += $"<b>Upgrades:</b> {playerData.Upgrades.Count}\r\n";
            playerData.Upgrades.ForEach(upgrade =>
            {
                playerDataText += $"<b>{upgrade.One.name}[{upgrade.Two}]:</b> {upgrade.One.Description}\r\n";
            });
            playerDataText += $"<b>Published Papers:</b> {playerData.PublicationProgress}/4\r\n";
            playerData.Papers.ForEach(paper =>
            {
                playerDataText += $"<b>{paper.paperTitle}</b> published at {paper.publicationName}\r\n";
            });
            cvText.text = playerDataText;
        }
    }
}
