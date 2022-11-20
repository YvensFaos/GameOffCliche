using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Events
{
    [CreateAssetMenu(fileName = "Gameplay Events", menuName = "Cliche/Events/Gameplay Events", order = 0)]
    public class GameplayEventsSO : ScriptableObject
    {
        public UnityAction<Data.BaseDataBehavior> OnNewDataCreated { get; set; }
        public void InvokeOnNewDataCreated(Data.BaseDataBehavior newData) => this.OnNewDataCreated?.Invoke(newData);
    }
}
