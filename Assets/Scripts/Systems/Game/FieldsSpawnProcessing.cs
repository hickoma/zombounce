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
        private EcsFilter<InFieldEvent> _inFieldEventFilter;
        private EcsFilter<UnityPrefabComponent> _unityPrefabFilter;

        private PoolContainer[] _poolContainers;
        private List<Field> _path;

        //z size of field
        private float _groundSize;

        //settable fields from starter
        public GameObject[] Prefabs;
        public int SpawnCount;
        public int InitialPoolSize;

        public void Initialize()
        {
            _path = new List<Field>(InitialPoolSize);
            AddRandomPath();
            InitPoolAndSpawnFirst();
        }
        
        public void Destroy()
        {
            _poolContainers = null;
            _path.Clear();
            _path = null;
            Prefabs = null;
        }

        public void Run()
        {
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
            _poolContainers = new PoolContainer[Prefabs.Length];
            for (int i = 0; i < Prefabs.Length; i++)
            {
                _poolContainers[i] = PoolContainer.CreatePool(Prefabs[i], parent);
            }
            
            var firstObject = PopOrSpawnById(0);

            //init ground size
            _groundSize = firstObject.PoolTransform.FindRecursiveByTag(Tag.Ground).localScale.y;
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

            var field = _path[id];

            if (!field.IsOnScene)
            {
                var obj = _poolContainers[field.PrefabId].Get();
                obj.PoolTransform.position = new Vector3(0f, 0f, id * _groundSize);
                obj.PoolTransform.gameObject.SetActive(true);
                field.PoolObject = obj;
                field.IsOnScene = true;
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

            var field = _path[id];
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
    }
}