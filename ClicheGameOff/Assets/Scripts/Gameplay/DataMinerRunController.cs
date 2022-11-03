using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Gameplay
{
    //Tick event delegate: called every tick.
    public delegate void RunTick(float currentTime);
    
    public class DataMinerRunController : MonoBehaviour
    {
        [SerializeField]
        private List<DataSpawner> spawners;
        [SerializeField]
        private float runTime;

        private event RunTick RunTickEvents;
        private float currentTime;

        private Dictionary<DataType, int> collectedData;

        public void OnEnable()
        {
            GameManager.Instance.SetDataMinerRunController(this);
        }

        public void StartRun()
        {
            StopAllCoroutines();
            StartCoroutine(RunCoroutine());
        }

        private IEnumerator RunCoroutine()
        {
            InitializeCollectedData();
            spawners.ForEach(spawner => spawner.StartSpawner());
            currentTime = runTime;
            while (currentTime >= 0)
            {
                //Wait for one frame
                yield return 0;

                RunTickEvents?.Invoke(currentTime);
                currentTime -= Time.deltaTime;
            }
            spawners.ForEach(spawner => spawner.StopSpawner());
        }

        private void InitializeCollectedData()
        {
            collectedData = new Dictionary<DataType, int>();
        }

        public void AddTickEvent(RunTick newEvent)
        {
            RunTickEvents += newEvent;
        }

        public void RemoveTickEvent(RunTick removeEvent)
        {
            RunTickEvents -= removeEvent;
        }

        public void CollectData(BaseDataBehavior data)
        {
            if (collectedData == null)
            {
                //BOOM!
                Debug.LogError("Collected Data is null!");
                InitializeCollectedData();
            }

            if (!collectedData.ContainsKey(data.Type))
            {
                collectedData.Add(data.Type, 0);
            }

            collectedData[data.Type]++;
        }
    }
}
