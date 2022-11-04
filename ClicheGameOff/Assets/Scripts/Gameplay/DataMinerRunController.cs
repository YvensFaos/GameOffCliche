using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Data;
using UnityEngine;

namespace Gameplay
{
    //Tick event delegate: called every tick.
    public delegate void RunTick(float currentTime, float normalizedTime);
    //Finished run event: called when the coroutine is about to end and stop all spawners.
    public delegate void FinishRun();
    
    public class DataMinerRunController : MonoBehaviour
    {
        [SerializeField]
        private List<DataSpawner> spawners;
        [SerializeField]
        private float runTime;

        private event RunTick RunTickEvents;
        private event FinishRun FinishRunEvents;
        private float currentTime;
        
        //Finished run variables
        private string runTextResults;
        private int runGoodData;
        private int runBadData;
        
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

                RunTickEvents?.Invoke(currentTime, currentTime / runTime);
                currentTime -= Time.deltaTime;
            }
            spawners.ForEach(spawner => spawner.StopSpawner());
            runGoodData = 0;
            runBadData = 0;
            
            //Add data to the player
            var stringBuilder = new StringBuilder();
            foreach (var pair in collectedData)
            {
                var dataType = pair.Key;
                stringBuilder.AppendLine($"<color=#{ColorUtility.ToHtmlStringRGB(dataType.typeColor)}>{dataType.GetName()} x{pair.Value}</color>");
                switch (dataType.qualifier)
                {
                    case DataQualifier.Good:
                        runGoodData += pair.Value;
                        break;
                    case DataQualifier.Bad:
                        runBadData += pair.Value;
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

        public void AddTickEvent(RunTick newEvent)
        {
            RunTickEvents += newEvent;
        }

        public void RemoveTickEvent(RunTick removeEvent)
        {
            RunTickEvents -= removeEvent;
        }

        public void AddFinishEvent(FinishRun newFinishEvent)
        {
            FinishRunEvents += newFinishEvent;
        }

        public void RemoveFinishEvent(FinishRun removeFinishEvent)
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

            //Check if any of this DataType has been collected yet
            if (!collectedData.ContainsKey(data.Type))
            {
                //If not, then create a new entry for that type starting with 0 counts.
                //Starts with 0 to avoid branching.
                collectedData.Add(data.Type, 0);
            }
            collectedData[data.Type]++;
        }

        public string GetResults() => runTextResults;
    }
}