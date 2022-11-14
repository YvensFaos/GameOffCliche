using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameConstants))]
    public class GameConstantsEditor : UnityEditor.Editor
    {
        private string lastGeneratedCliche;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var constants = (GameConstants)target;

            // ReSharper disable once InvertIf
            if (GUILayout.Button("Generate Random Cliche"))
            {
                lastGeneratedCliche = constants.GetRandomTitle();
                Debug.Log(lastGeneratedCliche);
                
            }
            GUILayout.Label($"Generated Cliche: {lastGeneratedCliche}.");
        }
    }
}