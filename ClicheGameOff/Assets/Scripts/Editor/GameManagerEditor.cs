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
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameManager = (GameManager)target;
            
            GUILayout.Space(10.0f);

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
                gameManager.DeleteSave();
            }
        }
    }
}