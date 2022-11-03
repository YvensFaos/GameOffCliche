using System.Collections;
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
        }
        
        private IEnumerator SpawnerCoroutine()
        {
            while (online)
            {
                yield return new WaitForSeconds(spawnTime);
                var selfTransform = transform;
                var spawnNumber = Random.Range(1, spawnRate);
                var position = RandomPointUtils.GetRandomPointWithBox(walkableArea);
                for (var i = 0; i < spawnNumber; i++)
                {
                    var data = Instantiate(RandomHelper<BaseDataBehavior>.GetRandomFromList(spawnTypes.dataBehaviors),
                        position, Quaternion.identity, selfTransform);
                    data.Initialize(walkableArea, spawnTypes.GetRandomDataTypeFromList());
                }
            }
        }
    }
}