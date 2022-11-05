using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Constants", menuName = "Cliche/Game Constants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        public int initialHardDriveSize = 10;
    }
}