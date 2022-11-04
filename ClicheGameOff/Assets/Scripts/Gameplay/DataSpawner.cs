using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class DataSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool online;
        [SerializeField] private float spawnTime;
        [SerializeField] private float spawnRate;
        [SerializeField] private BoxCollider walkableArea;
        
        [Header("References")]
        [SerializeField] private DataSpawnerList spawnTypes;

        [SerializeField] private List<BaseDataBehavior> currentData;

        public void StartSpawner()
        {
            online = true;
            StartCoroutine(SpawnerCoroutine());
        }

        public void StopSpawner()
        {
            online = false;
            //Safety check: stop coroutines right away in case this is called between a WaitForSeconds execution.
            StopAllCoroutines();
            
            //Kill uncollected data behaviors
            foreach (var dataBehavior in currentData.Where(dataBehavior => dataBehavior != null))
            {
                Destroy(dataBehavior.gameObject, 0.1f);
            }
        }
        
        private IEnumerator SpawnerCoroutine()
        {
            currentData = new List<BaseDataBehavior>();
            while (online)
            {
                var selfTransform = transform;
                var spawnNumber = Random.Range(1, spawnRate);
                var position = RandomPointUtils.GetRandomPointWithBox(walkableArea);
                
                for (var i = 0; i < spawnNumber; i++)
                {
                    var data = Instantiate(RandomHelper<BaseDataBehavior>.GetRandomFromList(spawnTypes.dataBehaviors),
                        position, Quaternion.identity, selfTransform);
                    data.Initialize(walkableArea, spawnTypes.GetRandomDataTypeFromList());
                    currentData.Add(data);
                }
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }
}