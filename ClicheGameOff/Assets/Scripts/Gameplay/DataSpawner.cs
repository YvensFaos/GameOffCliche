using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Events;
using UnityEngine;
using Utils;
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
        [SerializeField] private float spawnAnimationTimer = 0.5f;
        [SerializeField] private float destroyAnimationTimer = 0.5f;
        [SerializeField] private float destroyAnimationOffset = -10.0f;
        [SerializeField] private List<BaseDataBehavior> currentData;

        [Header("Scriptable Objects Refs")]
        [SerializeField] private GameplayEventsSO gameplayEventsSo;

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
                dataBehavior.transform.DOMoveY(destroyAnimationOffset - dataBehavior.transform.localScale.y / 2.0f, destroyAnimationTimer).OnComplete(() =>
                {
                    Destroy(dataBehavior.gameObject, 0.25f);
                });
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
                    position = TransformUtils.GetPointOnTheNavMesh(position);
                    positionPlaces.Add(position);
                    position.y += particlesYOffset;
                    var particles = Instantiate(spawnParticles, position, Quaternion.identity, selfTransform);
                    particles.Play();
                }
                yield return new WaitForSeconds(particlesTime);
                
                for (var i = 0; i < spawnNumber; i++)
                {
                    var dataToInstantiate = RandomHelper<BaseDataBehavior>.GetRandomFromList(SpawnerList.dataBehaviors);
                    var positionUnderground = positionPlaces[i];
                    positionUnderground.y -= dataToInstantiate.DataHeight;
                        
                    var data = Instantiate(dataToInstantiate,
                        positionUnderground, Quaternion.identity, selfTransform);
                    data.Initialize(walkableArea, SpawnerList.GetRandomDataTypeFromList());
                    currentData.Add(data);

                    data.transform.DOMoveY(positionPlaces[i].y + dataToInstantiate.transform.localScale.y / 2.0f, spawnAnimationTimer).OnComplete(() =>
                    {
                        data.StartBehavior();
                        gameplayEventsSo.InvokeOnNewDataCreated(data);
                    });
                }
                yield return new WaitForSeconds(spawnTime);
            }
        }

        public DataSpawnerList SpawnerList => spawnerList;
        public List<BaseDataBehavior> GetCurrentDataList => currentData;
    }
}