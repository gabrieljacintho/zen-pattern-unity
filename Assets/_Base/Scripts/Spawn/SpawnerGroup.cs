using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Pool;
using FireRingStudio.Random;
using Sirenix.OdinInspector;
using Subtegral.WeightedRandom;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Spawn
{
    public class SpawnerGroup : MonoBehaviour
    {
        [SerializeField] private List<WeightedValue<Spawner>> _spawners;
        [SerializeField] private string _spawnersId;
        [SerializeField, Range(0f, 1f)] private float _defaultWeight = 1f;
        [SerializeField] private bool _includeInactive;
        
        [Header("Settings")]
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private IntReference _maxInstances = new(-1);
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private IntReference _maxUnreleasedInstances = new(-1);

        private WeightedRandom<WeightedValue<Spawner>> _random;

        public bool IncludeInactive
        {
            get => _includeInactive;
            set => SetIncludeInactive(value);
        }
        public int InstancesCount => GetInstancesCount(_includeInactive);
        public int UnreleasedInstancesCount => GetUnreleasedInstancesCount(_includeInactive);
        
        private int MaxInstances => _maxInstances?.Value ?? 0;
        private int MaxUnreleasedInstances => _maxUnreleasedInstances?.Value ?? 0;

        private WeightedRandom<WeightedValue<Spawner>> Random
        {
            get
            {
                if (_random == null)
                {
                    UpdateRandom();
                }

                return _random;
            }
        }
        
        
        private void Awake()
        {
            LoadSpawners();
        }

        public List<PooledObject> Spawn(bool invisible = false)
        {
            if (!CanSpawn())
            {
                return null;
            }

            List<Spawner> spawners = GetRandomSpawners(_includeInactive);
            foreach (Spawner spawner in spawners)
            {
                if (!spawner.CanSpawn())
                {
                    continue;
                }

                List<PooledObject> newInstances = spawner.Spawn(invisible);
                if (newInstances.Count > 0)
                {
                    return newInstances;
                }
            }

            return null;
        }

        public void InvokeSpawn()
        {
            Spawn();
        }
        
        public void InvokeSpawnInvisible()
        {
            Spawn(true);
        }
        
        [Button]
        public List<PooledObject> SpawnQuantity(int quantity, bool invisible = false)
        {
            List<PooledObject> instances = new List<PooledObject>();
            for (int i = 0; i < quantity; i++)
            {
                List<PooledObject> newInstances = Spawn(invisible);
                if (newInstances.Count > 0)
                {
                    instances.AddRange(newInstances);
                }
                else
                {
                    break;
                }
            }

            return instances;
        }
        
        public void InvokeSpawnQuantity(int quantity)
        {
            SpawnQuantity(quantity);
        }
        
        public void InvokeSpawnQuantityInvisible(int quantity)
        {
            SpawnQuantity(quantity, true);
        }
        
        public List<PooledObject> SpawnAll(bool invisible = false)
        {
            List<PooledObject> allInstances = new List<PooledObject>();
            if (!CanSpawn())
            {
                return allInstances;
            }
                
            List<Spawner> spawners = GetRandomSpawners(_includeInactive);
            foreach (Spawner spawner in spawners)
            {
                while (spawner.CanSpawn())
                {
                    List<PooledObject> newInstances = spawner.Spawn(invisible);
                    if (newInstances.Count == 0)
                    {
                        break;
                    }
                    
                    allInstances.AddRange(newInstances);
                    if (!CanSpawn())
                    {
                        return allInstances;
                    }
                }
            }

            return allInstances;
        }
        
        public void InvokeSpawnAll()
        {
            SpawnAll();
        }
        
        public void InvokeSpawnAllInvisible()
        {
            SpawnAll(true);
        }

        public void ResetInstancesCount()
        {
            List<Spawner> spawners = GetSpawners(true);
            foreach (Spawner spawner in spawners)
            {
                spawner.ResetInstancesCount();
            }
        }
        
        public void ReleaseInstancesToPool()
        {
            List<Spawner> spawners = GetSpawners(true);
            foreach (Spawner spawner in spawners)
            {
                spawner.ReleaseInstancesToPool();
            }
        }

        public void SetActiveSpawners(bool value)
        {
            List<Spawner> spawners = GetSpawners(true);
            foreach (Spawner spawner in spawners)
            {
                spawner.gameObject.SetActive(value);
            }
        }
        
        public void SetIncludeInactive(bool value)
        {
            _includeInactive = value;
        }
        
        public bool CanSpawn()
        {
            if (Random == null || _spawners == null)
            {
                return false;
            }

            if (MaxInstances >= 0 && InstancesCount >= MaxInstances)
            {
                return false;
            }

            if (MaxUnreleasedInstances >= 0 && UnreleasedInstancesCount >= MaxUnreleasedInstances)
            {
                return false;
            }

            List<Spawner> spawners = GetRandomSpawners(_includeInactive);
            foreach (Spawner spawner in spawners)
            {
                if (spawner.CanSpawn())
                {
                    return true;
                }
            }

            return false;
        }

        private List<Spawner> GetSpawners(bool includeInactive)
        {
            List<Spawner> spawners = new List<Spawner>();

            if (_spawners == null)
            {
                return spawners;
            }
            
            foreach (WeightedValue<Spawner> weightedSpawner in _spawners)
            {
                if (weightedSpawner.Variants == null)
                {
                    continue;
                }

                foreach (Spawner spawner in weightedSpawner.Variants)
                {
                    if (includeInactive || spawner.isActiveAndEnabled)
                    {
                        spawners.Add(spawner);
                    }
                }
            }

            return spawners;
        }

        private List<Spawner> GetRandomSpawners(bool includeInactive)
        {
            List<Spawner> spawners = GetSpawners(includeInactive);
            spawners.Shuffle();

            return spawners;
        }

        private int GetInstancesCount(bool includeInactive)
        {
            List<Spawner> spawners = GetSpawners(includeInactive);
            int instancesCount = 0;
            foreach (Spawner spawner in spawners)
            {
                instancesCount += spawner.InstancesCount;
            }

            return instancesCount;
        }
        
        private int GetUnreleasedInstancesCount(bool includeInactive)
        {
            List<Spawner> spawners = GetSpawners(includeInactive);
            int instancesCount = 0;
            foreach (Spawner spawner in spawners)
            {
                instancesCount += spawner.UnreleasedInstancesCount;
            }

            return instancesCount;
        }

        private void UpdateRandom()
        {
            if (_spawners == null || _spawners.Count == 0)
            {
                _random = null;
                return;
            }

            _random = new WeightedRandom<WeightedValue<Spawner>>();
            _random.AddRange(_spawners);
        }

        private void LoadSpawners()
        {
            if (_spawners == null)
            {
                _spawners = new List<WeightedValue<Spawner>>();
            }
            
            if (string.IsNullOrEmpty(_spawnersId))
            {
                return;
            }
            
            List<GameObject> spawnerObjects = GameObjectID.FindGameObjectsWithID(_spawnersId, true);
            foreach (GameObject spawnerObject in spawnerObjects)
            {
                Spawner spawner = spawnerObject.GetComponent<Spawner>();
                if (spawner != null && !_spawners.Exists(x => x.FirstVariant == spawner))
                {
                    WeightedValue<Spawner> weightedSpawner = new (spawner, _defaultWeight);
                    _spawners.Add(weightedSpawner);
                }
            }
        }
    }
}