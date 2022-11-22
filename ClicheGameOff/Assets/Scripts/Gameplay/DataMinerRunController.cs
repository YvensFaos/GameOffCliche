using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Data;
using UnityEngine;

namespace Gameplay
{
    //Tick event delegate: called every tick.
    public delegate void RunTickDelegate(float currentTime, float normalizedTime);
    //Run every time a new data behavior is collected.
    public delegate void CollectDataDelegate(BaseDataBehavior dataBehavior, float normalizedHardDriveUsed);
    //Finished run event: called when the coroutine is about to end and stop all spawners.
    public delegate void FinishRunDelegate();
    
    public class DataMinerRunController : MonoBehaviour
    {
        // [SerializeField]
        // private float runTime;

        private event RunTickDelegate RunTickEvents;
        private event CollectDataDelegate CollectDataEvents;
        private event FinishRunDelegate FinishRunEvents;
        
        private float currentTime;
        
        //Finished run variables
        private string runTextResults;
        private int runGoodData;
        private int runBadData;
        
        private float currentHardDrive;
        private float accumulatedHardDriveUse;
        private float normalizedHardDrive;
        
        private Dictionary<DataType, int> collectedData;

        public void Start()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.SetDataMinerRunController(this);
        }

        public void StartRun(DataSpawner spawner)
        {
            currentHardDrive = GameManager.Instance.CurrentPlayerData.HardDriveSize;
            accumulatedHardDriveUse = 0.0f;
            normalizedHardDrive = accumulatedHardDriveUse / currentHardDrive;
            StopAllCoroutines();
            StartCoroutine(RunCoroutine(spawner));
        }

        private IEnumerator RunCoroutine(DataSpawner spawner)
        {
            InitializeCollectedData();
            spawner.StartSpawner();
            var spawnerList = spawner.SpawnerList;
            currentTime = spawnerList.runLength;
            var runTime = spawnerList.runLength;
            var restTime = spawnerList.RestTime;
            
            while (currentTime >= 0)
            {
                //Wait for one frame
                yield return 0;

                RunTickEvents?.Invoke(currentTime, currentTime / runTime);
                
                currentTime -= Time.deltaTime;
                
                if (currentTime < restTime)
                {
                    spawner.StopSpawner();
                }
            }
            spawner.KillRemainingData();
            
            runGoodData = 0;
            runBadData = 0;
            
            //Add data to the player
            var stringBuilder = new StringBuilder();
            foreach (var (dataType, value) in collectedData)
            {
                stringBuilder.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(dataType.typeColor)}>{dataType.GetName()} x{value}</color>");
                switch (dataType.qualifier)
                {
                    case DataQualifier.Good:
                        runGoodData += value;
                        break;
                    case DataQualifier.Bad:
                        runBadData += value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            runTextResults = stringBuilder.ToString();
            GameManager.Instance.ManagePlayerCollectedData(runGoodData, runBadData);
            FinishRunEvents?.Invoke();
        }

        private void InitializeCollectedData()
        {
            collectedData = new Dictionary<DataType, int>();
        }

        public void AddCollectDataEvent(CollectDataDelegate newEvent)
        {
            CollectDataEvents += newEvent;
        }

        public void RemoveCollectDataEvent(CollectDataDelegate removeEvent)
        {
            CollectDataEvents -= removeEvent;
        }
        
        public void AddTickEvent(RunTickDelegate newEvent)
        {
            RunTickEvents += newEvent;
        }

        public void RemoveTickEvent(RunTickDelegate removeEvent)
        {
            RunTickEvents -= removeEvent;
        }

        public void AddFinishEvent(FinishRunDelegate newFinishEvent)
        {
            FinishRunEvents += newFinishEvent;
        }

        public void RemoveFinishEvent(FinishRunDelegate removeFinishEvent)
        {
            FinishRunEvents -= removeFinishEvent;
        }

        public void CollectData(BaseDataBehavior data)
        {
            if (collectedData == null)
            {
                Debug.LogError("Collected Data is null!");
                InitializeCollectedData();
            }
            
            //Check hard drive
            if (accumulatedHardDriveUse + data.HardDriveUse <= currentHardDrive)
            {
                accumulatedHardDriveUse += data.HardDriveUse;
                normalizedHardDrive = accumulatedHardDriveUse / currentHardDrive;
                CollectDataEvents?.Invoke(data, normalizedHardDrive);
                //Check if any of this DataType has been collected yet
                if (!collectedData.ContainsKey(data.Type))
                {
                    //If not, then create a new entry for that type starting with 0 counts.
                    //Starts with 0 to avoid branching.
                    collectedData.Add(data.Type, 0);
                }
                collectedData[data.Type] += data.HardDriveUse;
            }
            else
            {
                //Hard drive is full!
            }
        }

        public string GetResults() => runTextResults;
    }
}
