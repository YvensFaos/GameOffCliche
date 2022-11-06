using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        //Example
        private string testJson = "{\"goodData\":100,\"badData\":100,\"hardDriveSize\":100," +
                                  "\"upgrades\":[{\"one\":{\"instanceID\":-63214},\"two\":1}]}";

        private int addGoodData = 10;
        private int addBadData = 10;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameManager = (GameManager)target;
            
            GUILayout.Space(15.0f);
            GUILayout.Label("PERSISTENCE",  new GUIStyle{fontSize = 15, normal = { textColor = Color.white}});

            if (GUILayout.Button("Test Player Data JSON"))
            {
                Debug.Log(gameManager.CurrentPlayerData.ToJson());
            }

            GUILayout.Label("Test Read Player Data JSON");
            testJson = GUILayout.TextArea(testJson);
            
            if (GUILayout.Button("Read Player Data JSON"))
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