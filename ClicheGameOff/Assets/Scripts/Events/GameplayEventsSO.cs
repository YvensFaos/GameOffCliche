using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(fileName = "Gameplay Events", menuName = "Cliche/Events/Gameplay Events", order = 0)]
    public class GameplayEventsSO : ScriptableObject
    {
        public UnityAction<Data.BaseDataBehavior> OnNewDataCreated { get; set; }
        public void InvokeOnNewDataCreated(Data.BaseDataBehavior newData) => OnNewDataCreated?.Invoke(newData);

        public UnityAction OnRunStarted { get; set; }
        public void InvokeOnRunStarted() => OnRunStarted?.Invoke();

        public UnityAction OnRunEnded { get; set; }
        public void InvokeOnRunEnded() => OnRunEnded?.Invoke();
    }
}
