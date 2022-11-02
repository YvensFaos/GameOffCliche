using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "New Data Type", menuName = "Cliche/Data Type", order = 0)]
    public class DataType : ScriptableObject
    {
        public string GetName() => name;
    }
}