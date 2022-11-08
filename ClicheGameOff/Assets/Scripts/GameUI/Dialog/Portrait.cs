using UnityEngine;

namespace GameUI.Dialog
{
    [CreateAssetMenu(fileName = "New Portrait", menuName = "Cliche/Portrait", order = 0)]
    public class Portrait : ScriptableObject
    {
        public Sprite sprite;

        public bool IsPortrait(string spriteName)
        {
            return name.Equals(spriteName);
        }
    }
}