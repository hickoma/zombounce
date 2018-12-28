using System.Collections.Generic;
using Data;
using LeopotamGroup.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 649

namespace Systems.Game
{
	public class FieldSpawnController : MonoBehaviour
    {
        private PoolContainer[] _poolContainers;
		private PoolContainer _zombiePool;
        private PoolContainer _coinsPool;
        private List<Field> _path;
        private List<int> _alreadyEnergy;
        private List<int> _alreadyCoins;

        //z size of field
        private float _groundSize;

        // all fields
        private List<GameObject> m_AllLevels;
        //settable fields from starter
		private List<GameObject> m_EasyLevelsPrefabs;
        private List<GameObject> m_NormalLevelsPrefabs;
        private List<GameObject> m_HardLevelsPrefabs;

		public GameObject[] EasyLevelsPrefabs
		{
			set
			{
                m_EasyLevelsPrefabs = new List<GameObject> (value);
			}
		}

        public GameObject[] NormalLevelsPrefabs
        {
            set
            {
                m_NormalLevelsPrefabs = new List<GameObject> (value);
            }
        }

        public GameObject[] HardLevelsPrefabs
        {
            set
            {
                m_HardLevelsPrefabs = new List<GameObject> (value);
            }
        }

		[HideInInspector] public int ForwardSpawnCount;
		[HideInInspector] public int BackwardSpawnCount;
		
		[HideInInspector] public int EnergySpawnEasyModeCount;
		[HideInInspector] public int EnergySpawnNormalModeCount;
		[HideInInspector] public int EnergySpawnHardModeCount;
		[HideInInspector] public int CoinSpawnCount;
		[HideInInspector] public GameObject[] ZombiePrefabs;
		[HideInInspector] public GameObject CoinPrefab;
		[HideInInspector] public GameObject[] FirstSessionLevels;

		[SerializeField]
		private Transform m_DefaultLevelTransform = null;

		private int ZombieSpawnCount
		{
			get
			{
                int currentZombieSpawn = 0;
                GameState.DifficultyMode difficultyMode = GameState.Instance.Difficulty;

                switch ((int)difficultyMode)
                {
                    case (int)(GameState.DifficultyMode.EASY):
                        currentZombieSpawn = EnergySpawnEasyModeCount;
                        break;

                    case (int)(GameState.DifficultyMode.NORMAL):
                        currentZombieSpawn = EnergySpawnNormalModeCount;
                        break;

                    case (int)(GameState.DifficultyMode.HARD):
                        currentZombieSpawn = EnergySpawnHardModeCount;
                        break;
                }

				return currentZombieSpawn;
			}
		}

        public void LateStart()
        {
            _path = new List<Field>(ForwardSpawnCount);
            _alreadyEnergy = new List<int>();
            _alreadyCoins = new List<int>();

            InitPoolAndSpawnFirst();

			// do not spawn fields on next field enter
//			GameEventsController.Instance.OnFieldEntered += CheckSpawn;
			GameEventsController.Instance.OnEnergyGathered += CheckEnergyEvents;
			GameEventsController.Instance.OnCoinGathered += CheckCoinEvents;

			GameEventsController.Instance.OnPlayerStopped += CheckSpawn;
        }

        public void Destroy()
        {
            _poolContainers = null;
            _zombiePool = null;
            _coinsPool = null;
            _path.Clear();
            _path = null;
            _alreadyEnergy.Clear();
            _alreadyEnergy = null; 
            _alreadyCoins.Clear();
            _alreadyCoins = null;
            m_AllLevels = null;
			m_EasyLevelsPrefabs = null;
            m_NormalLevelsPrefabs = null;
            m_HardLevelsPrefabs = null;
        }

		private void CheckSpawn(float zPosition)
		{
			int id = (int) (zPosition / _groundSize);

			// do not react at -1 and 0 fields,
			// because they are spawned on scene by default,
			// all needed forward fields are spawned automatically at InitPool
			if (id > 0)
			{
				if (!CheckAndDeleteBackward(id))
				{
					SpawnBackward(id);
				}

				if (!CheckAndDeleteForward(id))
				{
					SpawnForward(id);
				}
			}
		}

        private void InitPoolAndSpawnFirst()
        {
            var parent = GameObject.FindGameObjectWithTag(Tag.FieldPool).transform;
            //create zombie pool
            _zombiePool = PoolContainer.CreatePool(ZombiePrefabs[0], parent);

            //create coin pool
            _coinsPool = PoolContainer.CreatePool(CoinPrefab, parent);

            //create fields pool
            _poolContainers = new PoolContainer[m_EasyLevelsPrefabs.Count 
                                                + m_NormalLevelsPrefabs.Count 
                                                + m_HardLevelsPrefabs.Count];
            m_AllLevels = new List<GameObject>();
            m_AllLevels.AddRange(m_EasyLevelsPrefabs);
            m_AllLevels.AddRange(m_NormalLevelsPrefabs);
            m_AllLevels.AddRange(m_HardLevelsPrefabs);
            for (int i = 0; i < _poolContainers.Length; i++)
            {
                _poolContainers[i] = PoolContainer.CreatePool(new List<GameObject>(m_AllLevels)[i], parent);
            }

			// spawn zombies for default level
			SpawnZombie (0, m_DefaultLevelTransform);

            //init ground size
            //            _groundSize = firstObject.PoolTransform.FindRecursiveByTag(Tag.Ground).localScale.y;
            _groundSize = 30f;

			// add any level, it won't be generated, because level0 is already presented
			_path.Add (new Field (0));

            // -1 and 0 fields are already on scene so spawn some more fields forward
            for (int i = 0; i < ForwardSpawnCount; i++)
            {
                PopOrSpawnById(i + 1);
            }
        }

        private void AddRandomPath(int pathLength)
        {
            for (int i = 0; i < pathLength; i++)
            {
                if (Systems.GameState.Instance.SessionsCount == 1 && _path.Count <= FirstSessionLevels.Length)
				{
					GameObject orderedTutorialLevel = FirstSessionLevels[_path.Count - 1];
					int index = m_AllLevels.IndexOf (orderedTutorialLevel);
					_path.Add (new Field (index));
				}
                else if (GameState.Instance.Difficulty == GameState.DifficultyMode.EASY)
				{
                    int randomEasyOrNormalLevelNumber = Random.Range(0, m_EasyLevelsPrefabs.Count
                                                                        + m_NormalLevelsPrefabs.Count);
                    GameObject randomEasyOrNormalLevel;

                    if (randomEasyOrNormalLevelNumber < m_EasyLevelsPrefabs.Count)
                    {
                        randomEasyOrNormalLevel = m_EasyLevelsPrefabs[randomEasyOrNormalLevelNumber];
                    }
                    else
                    {
                        randomEasyOrNormalLevel = m_NormalLevelsPrefabs[randomEasyOrNormalLevelNumber - m_EasyLevelsPrefabs.Count];
                    }

					int index = m_AllLevels.IndexOf (randomEasyOrNormalLevel);
					_path.Add (new Field (index));
				}
                else if (GameState.Instance.Difficulty == GameState.DifficultyMode.NORMAL)
                {
                    GameObject randomNormalLevel = m_NormalLevelsPrefabs[Random.Range (0, m_NormalLevelsPrefabs.Count)];
                    _path.Add (new Field (m_AllLevels.IndexOf(randomNormalLevel)));
                }
                else if (GameState.Instance.Difficulty == GameState.DifficultyMode.HARD)
                {
                    int randomNormalOrHardLevelNumber = Random.Range(0, m_NormalLevelsPrefabs.Count
                                                                        + m_HardLevelsPrefabs.Count);
                    GameObject randomNormalOrHardLevel;

                    if (randomNormalOrHardLevelNumber < m_NormalLevelsPrefabs.Count)
                    {
                        randomNormalOrHardLevel = m_NormalLevelsPrefabs[randomNormalOrHardLevelNumber];
                    }
                    else
                    {
                        randomNormalOrHardLevel = m_HardLevelsPrefabs[randomNormalOrHardLevelNumber - m_NormalLevelsPrefabs.Count];
                    }

					int index = m_AllLevels.IndexOf (randomNormalOrHardLevel);
					_path.Add (new Field (index));
                }
            }
        }

        private void SpawnForward(int currentId)
        {
            int startIndex = currentId + 1;
            int endIndex = startIndex + ForwardSpawnCount;

            for (int i = startIndex; i < endIndex; i++)
            {
                PopOrSpawnById(i);
            }
        }

        private void SpawnBackward(int currentId)
        {
            int startIndex = currentId - 1;

            // do not spawn -1 and 0 fields
            if (startIndex < 1) return;

			int endIndex = startIndex - BackwardSpawnCount;

            for (int i = startIndex; i >= 0 && i > endIndex; i--)
            {
                PopOrSpawnById(i);
            }
        }

        private bool CheckAndDeleteForward(int currentId)
        {
            int checkedIndex = currentId + ForwardSpawnCount + 1;
            return DespawnWithId(checkedIndex);
        }

        private bool CheckAndDeleteBackward(int currentId)
        {
			int checkedIndex = currentId - BackwardSpawnCount - 1;
            return DespawnWithId(checkedIndex);
        }

        private IPoolObject PopOrSpawnById(int id)
        {
            if (id >= _path.Count)
            {
                AddRandomPath(id - _path.Count + 1);
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

                SpawnZombie(id, obj.PoolTransform);
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

        private void DespawnZombie(IPoolObject obj)
        {
            if (obj.PoolTransform.gameObject.activeInHierarchy)
            {
                _zombiePool.Recycle(obj);
            }
        }

        private void SpawnZombie(int id, Transform poolTransform)
        {
            if (_alreadyEnergy.Contains(id)) return;

            _alreadyEnergy.Add(id);
			List<Transform> spawnPoints = FindAllChildrenRecursiveWithTag(poolTransform, Tag.ZombieSpawn);
            spawnPoints.Shuffle();

			for (int i = 0; i < spawnPoints.Count && i < ZombieSpawnCount; i++)
            {
                Transform point = spawnPoints[i];
				IPoolObject poolingObj = _zombiePool.Get();
                poolingObj.PoolTransform.position = point.transform.position;

				// set random zombie sprite
				// need to refactor it
				SpriteRenderer zombieSpriteRenderer = poolingObj.PoolTransform.GetComponentInChildren<SpriteRenderer> ();
				int randomZombieNumber = Random.Range (0, ZombiePrefabs.Length);
                zombieSpriteRenderer.sprite = ZombiePrefabs [randomZombieNumber].GetComponentInChildren<SpriteRenderer> ().sprite;

				// finally activate
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
            List<Transform> spawnPoints = FindAllChildrenRecursiveWithTag(obj.PoolTransform, Tag.CoinSpawn);
            spawnPoints.Shuffle();
            
            for (int i = 0; i < spawnPoints.Count && i < CoinSpawnCount; i++)
            {
                var point = spawnPoints[i];
                var poolingObj = _coinsPool.Get();
                poolingObj.PoolTransform.position = point.transform.position;
                poolingObj.PoolTransform.gameObject.SetActive(true);
            }
        }
        
		private void CheckEnergyEvents(IPoolObject energy)
        {
			DespawnZombie(energy);
        }
        
		private void CheckCoinEvents(IPoolObject coin)
        {
			DespawnCoin(coin);
        }
        
        private Field GetField(int id)
        {
            if (id >= _path.Count)
            {
                AddRandomPath(id - _path.Count + 1);
            }

            return _path[id];
        }

        private List<Transform> FindAllChildrenRecursiveWithTag(Transform inspectedObject, string tag)
        {
            List<Transform> childrenWithTag = new List<Transform>();

            if (inspectedObject.childCount > 0)
            {
                for (int i = 0; i < inspectedObject.childCount; i++)
                {
                    Transform inspectedChild = inspectedObject.GetChild(i);

                    // check child
                    if (inspectedChild.CompareTag(tag))
                    {
                        childrenWithTag.Add(inspectedChild);
                    }

                    // go deeper and look in child's children
                    childrenWithTag.AddRange(FindAllChildrenRecursiveWithTag(inspectedChild, tag));
                }
            }

            return childrenWithTag;
        }
    }
}
