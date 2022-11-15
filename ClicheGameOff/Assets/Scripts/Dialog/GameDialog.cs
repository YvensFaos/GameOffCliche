using UnityEngine;
using Yarn.Unity;

namespace Dialog
{
    [CreateAssetMenu(fileName = "New Game Dialog", menuName = "Cliche/Game Dialog", order = 0)]
    public class GameDialog : ScriptableObject
    {
        public YarnProject yarnProject;
        public string startNode;
    }
}