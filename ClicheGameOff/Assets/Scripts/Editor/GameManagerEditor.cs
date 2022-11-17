using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        //Example
        private string testJson =
            $"{{\"goodData\":32,\"badData\":22,\"hardDriveSize\":10,\"stunResistance\":0.0,\"upgrades\":[],\"skills\":[]," +
            $"\"papers\":[],\"publicationProgress\":0,\"goodDataUsedSoFar\":0,\"badDataUsedSoFar\":0,\"upgradeNames\":[]," +
            $"\"skillNames\":[]}}";

        private int addGoodData = 10;
        private int addBadData = 10;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameManager = (GameManager)target;
            
            GUILayout.Space(15.0f);
            GUILayout.Label("PERSISTENCE",  new GUIStyle{fontSize = 15, normal = { textColor = Color.white}});

            if (GUILayout.Button("Print Player Data JSON"))
            {
                Debug.Log(gameManager.CurrentPlayerData.ToJson());
            }

            GUILayout.Label("Player Data JSON to be loaded:");
            testJson = GUILayout.TextArea(testJson);
            
            if (GUILayout.Button("Load Player Data JSON"))
            {
                gameManager.SetPlayerData(JsonUtility.FromJson<PlayerData>(testJson));
            }
            
            if (GUILayout.Button("Delete Player Data JSON"))
            {
                GameManager.DeleteSave();
            }
            
            GUILayout.Space(15.0f);
            GUILayout.Label("CHEAT",  new GUIStyle{fontSize = 15, normal = { textColor = Color.red}});
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Good Data: ");
            addGoodData = int.Parse(GUILayout.TextField(addGoodData.ToString()));
            if (GUILayout.Button($"Add {addGoodData} Good Data"))
            {
                GameManager.Instance.ManagePlayerCollectedData(addGoodData, 0);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Bad Data: ");
            addBadData = int.Parse(GUILayout.TextField(addBadData.ToString()));
            if (GUILayout.Button($"Add {addGoodData} Bad Data"))
            {
                GameManager.Instance.ManagePlayerCollectedData(0, addBadData);
            }
            GUILayout.EndHorizontal();
        }
    }
}