using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(DataSpawnerList))]
    public class DataSpawnerListEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var dataSpawnerList = (DataSpawnerList)target;
            
            GUILayout.Label($"Total chance: {dataSpawnerList.TotalChance}.");
            base.OnInspectorGUI();
        }
    }
}