using System.Collections;
using System.Collections.Generic;
using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.Pool;
using FireRingStudio.Save;
using Sirenix.OdinInspector;
using Subtegral.WeightedRandom;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Spawn
{
    public class Spawner : MonoBehaviour, ISave
    {
        #region Variables

        [SerializeField] protected List<SpawnOption> _prefabs;
        [SerializeField] private List<PooledObject> _instances;

        [Header("Settings")]
        [Tooltip("Set negative to not limit.")]
        [SerializeField] protected IntReference _maxInstances = new(-1);
        [Tooltip("Set negative to not limit."), HideIf("_discountReleasedInstances")]
        [SerializeField] protected IntReference _maxUnreleasedInstances = new(-1);
        [Tooltip("Set negative to not limit.")]
        [SerializeField] protected Vector2 _distanceRange;
        [SerializeField] protected Transform _parent;

        [Header("Avoidance")]
        [SerializeField, Min(0f)] protected float _avoidanceRadius;
        [ShowIf("@_avoidanceRadius > 0f")]
        [SerializeField] protected LayerMask _avoidanceLayerMask;
        [ShowIf("@_avoidanceRadius > 0f")]
        [SerializeField] protected QueryTriggerInteraction _avoidanceTriggerInteraction;

        [Header("Advanced Settings")]
        [SerializeField, Min(1)] protected int _spawnAttempts = 16;
        [SerializeField] private bool _discountReleasedInstances;
        [SerializeField] private bool _releaseInstancesOnGameReset;
        [SerializeField] private bool _resetInstancesCountOnGameReset;
        [SerializeField] private bool _alwaysShowGizmos;

        [Header("Save")]
        [SerializeField] private string _saveKey = "spawner";

        private WeightedRandom<SpawnOption> _random;

        #endregion

        #region Properties

        public List<PooledObject> Instances
        {
            get
            {
                if (_instances == null)
                {
                    _instances = new List<PooledObject>();
                }

                return _instances;
            }
        }

        public int InstancesCount { get; set; }
        public int UnreleasedInstancesCount { get; set; }

        public string SaveKey => _saveKey;

        protected int MaxInstances => _maxInstances?.Value ?? -1;
        protected int MaxUnreleasedInstances => _maxUnreleasedInstances?.Value ?? -1;
        protected float MinSpawnDistance => Mathf.Max(_distanceRange.x, 0f);
        protected float MaxSpawnDistance => _distanceRange.y < 0f ? float.MaxValue : Mathf.Max(_distanceRange.y, MinSpawnDistance);
        protected Vector2 SpawnDistanceRange => new Vector2(MinSpawnDistance, MaxSpawnDistance);

        private WeightedRandom<SpawnOption> Random
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

        private string InstancesCountSaveKey => SaveKey + "-instances-count";
        private string UnreleasedInstancesCountSaveKey => SaveKey + "-instances-count-unreleased";
        private string InstanceSaveKey => SaveKey + "-instance";

        #endregion

        #region Events

        [Space]
        [FormerlySerializedAs("onSpawn")]
        public UnityEvent<PooledObject> OnSpawn;
        [FormerlySerializedAs("onSpawnAmount")]
        public UnityEvent<List<PooledObject>> OnSpawnQuantity;

        #endregion

        #region MonoBehaviour Methods

        protected virtual void Awake()
        {
            RegisterListeners();

            if (_instances != null && _instances.Count > 0)
            {
                this.DoOnNextFrame(() =>
                {
                    List<PooledObject> instances = new(_instances);
                    _instances.Clear();
                    instances.ForEach(AddInstance);
                });
            }
            else
            {
                _instances = new();
            }

            SaveManager.RegisterSave(this);
        }

        protected virtual void OnEnable()
        {
            UpdateRandom();
        }

        protected virtual void OnDrawGizmos()
        {
            if (!_alwaysShowGizmos)
            {
                return;
            }

            ShowGizmos();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            ShowGizmos();
        }

        private void ShowGizmos()
        {
            Vector3 center = transform.position;

            Gizmos.color = Color.white;

            if (MinSpawnDistance > 0f)
            {
                Gizmos.DrawWireSphere(center, MinSpawnDistance);
            }

            if (MaxSpawnDistance > MinSpawnDistance && MaxSpawnDistance < Mathf.Infinity)
            {
                Gizmos.DrawWireSphere(center, MaxSpawnDistance);
            }
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
        }

        protected virtual void OnDestroy()
        {
            UnregisterListeners();
            SaveManager.UnregisterSave(this);
        }

        #endregion

        #region Public Methods

        public List<PooledObject> Spawn(bool invisible = false)
        {
            if (!CanSpawn())
            {
                return new List<PooledObject>();
            }

            for (int i = 0; i < _spawnAttempts; i++)
            {
                if (TrySpawn(invisible, out List<PooledObject> instances))
                {
                    OnSpawn?.Invoke(instances[0]);
                    OnSpawnQuantity?.Invoke(instances);
                    return instances;
                }
            }

            return new List<PooledObject>();
        }
        
        public void InvokeSpawn()
        {
            Spawn();
        }

        public void InvokeSpawnInvisible()
        {
            Spawn(true);
        }
        
        [ShowIf("CanSpawn")]
        [Button]
        public virtual void SpawnQuantity(int quantity, bool invisible = false)
        {
            if (!CanSpawn())
            {
                return;
            }

            if (MaxInstances >= 0)
            {
                quantity = Mathf.Min(quantity, MaxInstances - InstancesCount);
            }

            IEnumerator Routine()
            {
                List<PooledObject> instances = new List<PooledObject>();
                for (int a = 0; a < quantity; a++)
                {
                    List<PooledObject> newInstances = Spawn(invisible);
                    if (newInstances.Count > 0)
                    {
                        instances.AddRange(newInstances);
                    }

                    yield return null;
                }

                OnSpawnQuantity?.Invoke(instances);
            }

            if (UpdateManager.Instance != null)
            {
                UpdateManager.Instance.StartCoroutine(Routine());
            }
        }

        public void InvokeSpawnQuantity(int quantity)
        {
            SpawnQuantity(quantity);
        }
        
        public void InvokeSpawnQuantityInvisible(int quantity)
        {
            SpawnQuantity(quantity, true);
        }
        
        [ShowIf("CanSpawn")]
        [Button]
        public void SpawnAll(bool invisible = false)
        {
            if (MaxInstances <= 0 && MaxUnreleasedInstances <= 0)
            {
                Debug.LogError("The max instances is zero or unlimited!", this);
                return;
            }

            int quantity = -1;
            if (MaxInstances > 0)
            {
                quantity = Mathf.Max(MaxInstances - InstancesCount, 0);
            }

            if (MaxUnreleasedInstances > 0)
            {
                int unreleasedQuantity = Mathf.Max(MaxUnreleasedInstances - UnreleasedInstancesCount, 0);
                quantity = quantity == -1 ? unreleasedQuantity : Mathf.Min(quantity, unreleasedQuantity);
            }

            if (quantity <= 0)
            {
                return;
            }

            SpawnQuantity(quantity, invisible);
        }
        
        public void InvokeSpawnAll()
        {
            SpawnAll();
        }
        
        public void InvokeSpawnAllInvisible()
        {
            SpawnAll(true);
        }

        public PooledObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, bool invisible = false)
        {
            PooledObject instance;
            if (invisible)
            {
                instance = prefab.GetInvisible(position, rotation, _parent);
            }
            else
            {
                instance = prefab.Get(position, rotation, _parent);
            }

            AddInstance(instance);

            return instance;
        }

        public void ReleaseInstancesToPool()
        {
            new List<PooledObject>(_instances).ForEach(instance => instance.ReleaseToPool());
        }

        public void ResetInstancesCount()
        {
            InstancesCount = 0;
            UnreleasedInstancesCount = 0;
        }

        public bool CanSpawn()
        {
            if (Random == null)
            {
                return false;
            }
            
            if (MaxInstances >= 0)
            {
                if (InstancesCount >= MaxInstances)
                {
                    return false;
                }
            }

            if (MaxUnreleasedInstances >= 0)
            {
                if (UnreleasedInstancesCount >= MaxUnreleasedInstances)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Protected Methods

        protected virtual bool TryGetSpawnPoint(GameObject prefab, bool invisible, out Vector3 point, out Quaternion rotation, out List<PooledObject> releasedObjects)
        {
            point = transform.position;
            rotation = transform.rotation;
            releasedObjects = new List<PooledObject>();

            if (SpawnDistanceRange != Vector2.zero)
            {
                float distance = UnityEngine.Random.Range(MinSpawnDistance, MaxSpawnDistance);
                point += UnityEngine.Random.insideUnitSphere * distance;
            }

            if (invisible && point.IsVisibleByCurrentCamera())
            {
                return false;
            }

            return CanSpawnAt(point, out releasedObjects);
        }

        protected bool CanSpawnAt(Vector3 point, out List<PooledObject> inactiveObjects)
        {
            inactiveObjects = new List<PooledObject>();

            if (_avoidanceRadius <= 0f || _avoidanceLayerMask.IsEmpty())
            {
                return true;
            }

            Collider[] results = UnityEngine.Physics.OverlapSphere(point, _avoidanceRadius, _avoidanceLayerMask, _avoidanceTriggerInteraction);

            foreach (Collider result in results)
            {
                PooledObject pooledObject = ComponentCacheManager.GetComponent<PooledObject>(result.gameObject);
                if (pooledObject == null || pooledObject.IsActive)
                {
                    return false;
                }

                inactiveObjects.Add(pooledObject);
            }

            return true;
        }

        #endregion

        #region Private Methods

        private bool TrySpawn(bool invisible, out List<PooledObject> instances)
        {
            instances = new List<PooledObject>();
            
            if (!TryGetSpawnOption(out SpawnOption spawnOption))
            {
                return false;
            }

            GameObject prefab = spawnOption.GetRandomVariant();
            if (prefab == null)
            {
                return false;
            }

            if (!TryGetSpawnPoint(prefab, invisible, out Vector3 spawnPoint, out Quaternion spawnRotation, out List<PooledObject> releasedObjects))
            {
                return false;
            }

            foreach (PooledObject releasedObject in releasedObjects)
            {
                releasedObject.gameObject.SetActive(false);
            }

            spawnRotation *= prefab.transform.rotation;

            int quantity = spawnOption.GetRandomQuantity();
            for (int i = 0; i < quantity; i++)
            {
                instances.Add(Spawn(prefab, spawnPoint, spawnRotation));
            }
            
            return quantity > 0;
        }

        private bool TryGetSpawnOption(out SpawnOption spawnOption)
        {
            spawnOption = null;
            if (Random == null)
            {
                return false;
            }
            
            spawnOption = Random.Next();
            
            return spawnOption != null;
        }

        private void AddInstance(PooledObject instance)
        {
            InstancesCount++;

            if (_instances.Contains(instance))
            {
                return;
            }

            _instances.Add(instance);

            AddUnreleasedInstance(instance);
        }

        private void AddUnreleasedInstance(PooledObject instance)
        {
            UnreleasedInstancesCount++;

            void OnRelease()
            {
                UnreleasedInstancesCount--;

                if (_discountReleasedInstances)
                {
                    InstancesCount--;
                }

                _instances.Remove(instance);

                instance.OnRelease.RemoveListener(OnRelease);
            }

            instance.OnRelease.AddListener(OnRelease);
        }

        private void UpdateRandom()
        {
            if (_prefabs == null || _prefabs.Count == 0)
            {
                _random = null;
                return;
            }

            _random = new WeightedRandom<SpawnOption>();
            _random.AddRange(_prefabs);
        }

        private void RegisterListeners()
        {
            if (_prefabs != null)
            {
                List<SpawnOption> valuesWithVariable = _prefabs.FindAll(value => value.WeightVariable != null);
                valuesWithVariable.ForEach(value => value.WeightVariable.Changed.Register(UpdateRandom));
            }
            
            GameManager.GameStarted += OnGameReset;
            GameManager.GameReset += OnGameReset;
        }

        private void UnregisterListeners()
        {
            if (_prefabs != null)
            {
                List<SpawnOption> valuesWithVariable = _prefabs.FindAll(value => value.WeightVariable != null);
                valuesWithVariable.ForEach(value => value.WeightVariable.Changed.Unregister(UpdateRandom));
            }
            
            GameManager.GameStarted -= OnGameReset;
            GameManager.GameReset -= OnGameReset;
        }

        private void OnGameReset()
        {
            if (_releaseInstancesOnGameReset)
            {
                ReleaseInstancesToPool();
            }

            if (_resetInstancesCountOnGameReset)
            {
                ResetInstancesCount();
            }
        }

        #endregion

        #region Save

        public void Save(ES3Settings settings)
        {
            ES3.Save(SaveKey, true, settings);
            ES3.Save(InstancesCountSaveKey, InstancesCount, settings);
            
            int unreleasedInstancesCount = 0;
            foreach (PooledObject instance in _instances)
            {
                if (!instance.IsActive)
                {
                    continue;
                }

                ES3.Save(GetInstanceSaveKey(unreleasedInstancesCount), instance.gameObject, settings);

                string poolTag = instance.Pool.Tag;
                ES3.Save(GetInstancePoolTagSaveKey(unreleasedInstancesCount), poolTag, settings);

                unreleasedInstancesCount++;
        }

            ES3.Save(UnreleasedInstancesCountSaveKey, unreleasedInstancesCount, settings);
        }

        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            if (!ES3.KeyExists(SaveKey, settings))
            {
                return;
            }

            ReleaseInstancesToPool();

            int unreleasedInstancesCount = ES3.Load(UnreleasedInstancesCountSaveKey, 0, settings);
            for (int i = 0; i < unreleasedInstancesCount; i++)
            {
                string poolTag = ES3.Load<string>(GetInstancePoolTagSaveKey(i), settings);
                ObjectPool pool = PoolManager.FindValidPoolWithTag(poolTag);

                PooledObject instance;
                string instanceSaveKey = GetInstanceSaveKey(i);
                if (pool != null)
                {
                    instance = LoadInstance(pool, instanceSaveKey, settings);
                }
                else
                {
                    instance = LoadInstance(poolTag, instanceSaveKey, settings);
                }

                AddInstance(instance);
            }

            InstancesCount = ES3.Load(InstancesCountSaveKey, 0, settings);
        }

        private PooledObject LoadInstance(ObjectPool pool, string saveKey, ES3Settings settings)
        {
            PooledObject instance = pool.Get();
            ES3.LoadInto(saveKey, instance.gameObject, settings);

            return instance;
        }

        private PooledObject LoadInstance(string poolTag, string saveKey, ES3Settings settings)
        {
            PooledObject instance = ES3.Load<GameObject>(saveKey, settings).GetComponent<PooledObject>();

            ObjectPool pool = PoolManager.GetPool(poolTag, null);
            pool.AddActiveInstance(instance);

            ES3.LoadInto(saveKey, instance.gameObject, settings);
            instance.transform.parent = pool.transform;

            return instance;
        }

        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(SaveKey, settings);

            ES3.DeleteKey(InstancesCountSaveKey, settings);
            int unreleasedInstancesCount = ES3.Load(UnreleasedInstancesCountSaveKey, 0, settings);
            ES3.DeleteKey(UnreleasedInstancesCountSaveKey, settings);

            for (int i = 0; i < unreleasedInstancesCount; i++)
            {
                ES3.DeleteKey(GetInstancePoolTagSaveKey(i), settings);
                ES3.DeleteKey(GetInstanceSaveKey(i), settings);
            }
        }

        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));

        private string GetInstanceSaveKey(int index) => InstanceSaveKey + "-" + index;

        private string GetInstancePoolTagSaveKey(int index) => GetInstanceSaveKey(index) + "-pool-tag";

        #endregion
    }
}