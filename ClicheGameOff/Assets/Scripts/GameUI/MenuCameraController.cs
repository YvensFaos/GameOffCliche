using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class MenuCameraController : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> objectsToDisable;

        public void Transition(GameObject transitionTo) => Toggle(transitionTo, false, true);
        public void TransitionBack(GameObject transitionFrom) => Toggle(transitionFrom, true, false);
        
        private void Toggle(GameObject transitionTo, bool disabled, bool transition)
        {
            objectsToDisable.ForEach(obj => obj.SetActive(disabled));
            transitionTo.SetActive(transition);
        }

    }
}
