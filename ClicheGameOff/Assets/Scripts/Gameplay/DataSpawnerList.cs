using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Data Spawner List", menuName = "Cliche/Data Spawner List", order = 0)]
    public class DataSpawnerList : ScriptableObject
    {
        [Header("Data Spawns")]
        [SerializeField]
        public List<BaseDataBehavior> dataBehaviors;
        [SerializeField]
        public List<DataTypeChancePair> dataTypes;
        [SerializeField]
        public DataType defaultType;
        [SerializeField]
        public ParticleSystem spawnParticle;
        
        [Header("Spawner Info")]
        [SerializeField] 
        public float particlesTime;
        [SerializeField]
        public float spawnTime;
        [SerializeField] 
        public float spawnRate;

        private float totalChance;

        private void OnEnable()
        {
            RecalculateTotalChance();
        }

        private void OnValidate()
        {
            RecalculateTotalChance();
        }

        private void RecalculateTotalChance()
        {
            totalChance = dataTypes.Sum(pair => pair.Two);
        }

        public DataType GetRandomDataTypeFromList()
        {
            var chance = Random.Range(0, TotalChance);
            var cumulativeChance = 0.0f;
            foreach (var pair in dataTypes)
            {
                if (chance <= pair.Two + cumulativeChance)
                {
                    return pair.One;
                }

                cumulativeChance += pair.Two;
            }
            return defaultType;
        }
        
        public float TotalChance => totalChance;
    }
}