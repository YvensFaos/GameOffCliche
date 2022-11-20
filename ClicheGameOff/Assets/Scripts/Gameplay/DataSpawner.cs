using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Utils;
using Gameplay.Events;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class DataSpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool online;
        [SerializeField] private float particlesTime;
        [SerializeField] private float spawnTime;
        [SerializeField] private float spawnRate;
        [SerializeField] private BoxCollider walkableArea;
        [SerializeField] private LayerMask floorMask;
        
        [Header("References")]
        [SerializeField] private DataSpawnerList spawnerList;
        [SerializeField] private ParticleSystem spawnParticles;
        [SerializeField] private float particlesYOffset;
        
        [Header("Run Data")]
        [SerializeField] private List<BaseDataBehavior> currentData;

        [Header("Scriptable Objects Refs")]
        [SerializeField] private GameplayEventsSO _gameplayEventsSO;

        public List<BaseDataBehavior> GetCurrentDataList => this.currentData;

        private void Start()
        {
            if (spawnerList != null)
            {
                SetDataSpawnerList(spawnerList);
            }
        }

        public void SetDataSpawnerList(DataSpawnerList dataSpawnerList)
        {
            spawnerList = dataSpawnerList;
            particlesTime = dataSpawnerList.particlesTime;
            spawnTime = dataSpawnerList.spawnTime;
            spawnRate = dataSpawnerList.spawnRate;
            spawnParticles = dataSpawnerList.spawnParticle;
        }
        
        public void StartSpawner()
        {
            online = true;
            StartCoroutine(SpawnerCoroutine());
        }

        public void StopSpawner(bool killRemnants = true)
        {
            online = false;
            //Safety check: stop coroutines right away in case this is called between a WaitForSeconds execution.
            StopAllCoroutines();

            if (!killRemnants) return;
            KillRemainingData();
        }

        public void KillRemainingData()
        {
            //Kill uncollected data behaviors
            foreach (var dataBehavior in currentData.Where(dataBehavior => dataBehavior != null))
            {
                //TODO change to use animations
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

                var positionPlaces = new List<Vector3>();
                for (var i = 0; i < spawnNumber; i++)
                {
                    var position = RandomPointUtils.GetRandomPointWithBox(walkableArea);
                    position = TransformUtils.GetPointOnTheGround(position, floorMask);
                    positionPlaces.Add(position);
                    position.y += particlesYOffset;
                    var particles = Instantiate(spawnParticles, position, Quaternion.identity, selfTransform);
                    particles.Play();
                }
                yield return new WaitForSeconds(particlesTime);
                
                for (var i = 0; i < spawnNumber; i++)
                {
                    var data = Instantiate(RandomHelper<BaseDataBehavior>.GetRandomFromList(SpawnerList.dataBehaviors),
                        positionPlaces[i], Quaternion.identity, selfTransform);
                    //TODO change to use animations
                    data.Initialize(walkableArea, SpawnerList.GetRandomDataTypeFromList());
                    currentData.Add(data);

                    this._gameplayEventsSO.InvokeOnNewDataCreated(data);
                }
                yield return new WaitForSeconds(spawnTime);
            }
        }

        public DataSpawnerList SpawnerList => spawnerList;
    }
}