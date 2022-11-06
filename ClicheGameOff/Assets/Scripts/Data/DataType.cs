using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Data Type", menuName = "Cliche/Data Type", order = 0)]
    public class DataType : ScriptableObject
    {
        public string GetName() => name;
        public Color typeColor;
        public DataQualifier qualifier;

        private void OnValidate()
        {
            if (GameManager.Instance != null && GameManager.Instance.Constants != null)
            {
                typeColor = GameManager.Instance.Constants.GetColorForQualifier(qualifier);
            }
        }
    }
}