using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace GameUI.Dialog
{
    public class DialogEventCaller : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent dialogEvent;
        [SerializeField]
        private UnityEvent dialogReverseEvent;
        
        [YarnCommand("callEvent")]
        public void CallEvent() {
            dialogEvent.Invoke();
        }
        
        [YarnCommand("callReverseEvent")]
        public void CallReverseEvent()
        {
            dialogReverseEvent.Invoke();
        }
    }
}
