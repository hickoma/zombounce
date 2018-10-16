using System.Collections.Generic;
using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using LeopotamGroup.Ecs.UnityIntegration;
using LeopotamGroup.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 649

namespace Systems.Game
{
    [EcsInject]
    public class FieldsSpawnProcessing : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter<InFieldEvent> _inFieldEventFilter = null;
        private EcsFilter<UnityPrefabComponent> _unityPrefabFilter = null;
        private EcsFilter<DespawnEnergyEvent> _despawnEnergyEventFilter = null;
        private EcsFilter<DespawnCoinEvent> _despawnCoinEventFilter = null;

        private PoolContainer[] _poolContainers;
        private PoolContainer _energyPool;
        private PoolContainer _coinsPool;
        private List<Field> _path;
        private List<int> _alreadyEnergy;
        private List<int> _alreadyCoins;

        //z size of field
        private float _groundSize;

        //settable fields from starter
        public GameObject[] Prefabs;
        public int SpawnCount;
        public int InitialPoolSize;

        public int EnergySpawnCount;
        public int CoinSpawnCount;
        public GameObject EnergyPrefab;
        public GameObject CoinPrefab;

        public void Initialize()
        {
            _path = new List<Field>(InitialPoolSize);
            _alreadyEnergy = new List<int>();
            _alreadyCoins = new List<int>();
            AddRandomPath();
            InitPoolAndSpawnFirst();
        }

        public void Destroy()
        {
            _poolContainers = null;
            _energyPool = null;
            _coinsPool = null;
            _path.Clear();
            _path = null;
            _alreadyEnergy.Clear();
            _alreadyEnergy = null; 
            _alreadyCoins.Clear();
            _alreadyCoins = null;
            Prefabs = null;
        }

        public void Run()
        {
            CheckEnergyEvents();
            CheckCoinEvents();

            for (int i = 0; i < _inFieldEventFilter.EntitiesCount; i++)
            {
                var enterEvent = _inFieldEventFilter.Components1[i];
                var id = (int) (enterEvent.ZPosition / _groundSize);

                if (!CheckAndDeleteBackward(id))
                {
                    SpawnBackward(id);
                }

                if (!CheckAndDeleteForward(id))
                {
                    SpawnForward(id);
                }

                _world.RemoveEntity(_inFieldEventFilter.Entities[i]);
            }
        }

        private void InitPoolAndSpawnFirst()
        {
            var parent = GameObject.FindGameObjectWithTag(Tag.FieldPool).transform;
            //create energy pool
            _energyPool = PoolContainer.CreatePool(EnergyPrefab, parent);

            //create coin pool
            _coinsPool = PoolContainer.CreatePool(CoinPrefab, parent);

            //create fields pool
            _poolContainers = new PoolContainer[Prefabs.Length];
            for (int i = 0; i < Prefabs.Length; i++)
            {
                _poolContainers[i] = PoolContainer.CreatePool(Prefabs[i], parent);
            }

            var firstObject = PopOrSpawnById(0);

            //init ground size
//            _groundSize = firstObject.PoolTransform.FindRecursiveByTag(Tag.Ground).localScale.y;
            _groundSize = 22f;
        }

        private void AddRandomPath()
        {
            for (int i = 0; i < InitialPoolSize; i++)
            {
                _path.Add(new Field(Random.Range(0, Prefabs.Length)));
            }
        }

        private void SpawnForward(int currentId)
        {
            var startIndex = currentId + 1;
            var endIndex = startIndex + SpawnCount;

            for (var i = startIndex; i < endIndex; i++)
            {
                PopOrSpawnById(i);
            }
        }

        private void SpawnBackward(int currentId)
        {
            var startIndex = currentId - 1;
            if (startIndex < 0) return;
            var endIndex = startIndex - SpawnCount;

            for (var i = startIndex; i >= 0 && i > endIndex; i--)
            {
                PopOrSpawnById(i);
            }
        }

        private bool CheckAndDeleteForward(int currentId)
        {
            var checkedIndex = currentId + SpawnCount + 1;
            return DespawnWithId(checkedIndex);
        }

        private bool CheckAndDeleteBackward(int currentId)
        {
            var checkedIndex = currentId - SpawnCount - 1;
            return DespawnWithId(checkedIndex);
        }

        private IPoolObject PopOrSpawnById(int id)
        {
            if (_path.Count <= id)
            {
                AddRandomPath();
                PopOrSpawnById(id);
            }

            var field = GetField(id);

            if (!field.IsOnScene)
            {
                var obj = _poolContainers[field.PrefabId].Get();
                obj.PoolTransform.position = new Vector3(0f, 0f, id * _groundSize);
                obj.PoolTransform.gameObject.SetActive(true);
                field.PoolObject = obj;
                field.IsOnScene = true;

                SpawnEnergy(id, obj);
                SpawnCoin(id, obj);

                return obj;
            }

            return null;
        }

        private bool DespawnWithId(int id)
        {
            if (id < 0 || _path.Count < id)
            {
                return false;
            }

            var field = GetField(id);
            if (!field.IsOnScene)
            {
                return false;
            }

            var container = _poolContainers[field.PrefabId];
            container.Recycle(field.PoolObject);
            field.PoolObject = null;
            field.IsOnScene = false;
            return true;
        }

        private void DespawnEnergy(IPoolObject obj)
        {
            if (obj.PoolTransform.gameObject.activeInHierarchy)
            {
                _energyPool.Recycle(obj);
            }
        }

        private void SpawnEnergy(int id, IPoolObject obj)
        {
            if (_alreadyEnergy.Contains(id)) return;

            _alreadyEnergy.Add(id);
            var spawnPoints = obj.PoolTransform.FindAllRecursiveByTag(Tag.EnergySpawn);
            spawnPoints.Shuffle();
            for (int i = 0; i < spawnPoints.Count && i < EnergySpawnCount; i++)
            {
                var point = spawnPoints[i];
                var poolingObj = _energyPool.Get();
                poolingObj.PoolTransform.position = point.transform.position;
                poolingObj.PoolTransform.gameObject.SetActive(true);
            }
        }

        private void DespawnCoin(IPoolObject obj)
        {
            if (obj.PoolTransform.gameObject.activeInHierarchy)
            {
                _coinsPool.Recycle(obj);
            }
        }
        
        private void SpawnCoin(int id, IPoolObject obj)
        {
            if (_alreadyCoins.Contains(id)) return;

            _alreadyCoins.Add(id);
            var spawnPoints = obj.PoolTransform.FindAllRecursiveByTag(Tag.CoinSpawn);
            spawnPoints.Shuffle();
            
            for (int i = 0; i < spawnPoints.Count && i < CoinSpawnCount; i++)
            {
                var point = spawnPoints[i];
                var poolingObj = _coinsPool.Get();
                poolingObj.PoolTransform.position = point.transform.position;
                poolingObj.PoolTransform.gameObject.SetActive(true);
            }
        }
        
        private void CheckEnergyEvents()
        {
            for (int i = 0; i < _despawnEnergyEventFilter.EntitiesCount; i++)
            {
                var entity = _despawnEnergyEventFilter.Entities[i];
                var poolObject = _despawnEnergyEventFilter.Components1[i].PoolObject;
                DespawnEnergy(poolObject);
                _world.RemoveEntity(entity);
            }
        }
        
        private void CheckCoinEvents()
        {
            for (int i = 0; i < _despawnCoinEventFilter.EntitiesCount; i++)
            {
                var entity = _despawnCoinEventFilter.Entities[i];
                var poolObject = _despawnCoinEventFilter.Components1[i].PoolObject;
                DespawnCoin(poolObject);
                _world.RemoveEntity(entity);
            }
        }
        
        private Field GetField(int id)
        {
            if (id > _path.Count - 1)
            {
                AddRandomPath();
            }

            return _path[id];
        }
    }
}