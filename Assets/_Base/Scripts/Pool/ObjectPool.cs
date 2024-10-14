using System;
using System.Collections;
using System.Collections.Generic;
using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        #region Variables

        [SerializeField] private string _tag;
        [SerializeField] private GameObject _prefab;
        [SerializeField, Min(0)] private int _initialSize;
        // TODO: Implement max size
        // TODO: Implement stealing mode (similar to FMOD stealing mode)

        [PropertySpace]
        [ShowInInspector, ReadOnly] private readonly List<PooledObject> _activeObjects = new();
        [ShowInInspector, ReadOnly] private readonly List<PooledObject> _inactiveObjects = new();
        
        private readonly Dictionary<PooledObject, Renderer[]> _renderers = new();

        #endregion

        #region Properties

        public string Tag => _tag;
        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        #endregion

        #region Events

        [Space]
        public UnityEvent<PooledObject> onObjectGot = new();
        public UnityEvent<PooledObject> onObjectReleased = new();

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            PoolManager.AddPool(this);
        }

        private void Start()
        {
            if (_initialSize > 0)
            {
                CreateInactiveInstances(_initialSize);
            }
        }

        private void OnDestroy()
        {
            PoolManager.RemovePool(this);
            DestroyAllInstances();
        }

        #endregion

        #region Public Methods

        public void Initialize(string tag, GameObject prefab, int initialSize = 0)
        {
            _tag = tag;
            _prefab = prefab;
            _initialSize = initialSize;
        }
        
        public PooledObject Get(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            PooledObject pooledObject = _inactiveObjects.FirstOrDefault();
            
            return Get(pooledObject, position, rotation, parent);
        }

        public PooledObject Get(Transform parent)
        {
            return Get(default, default, parent);
        }
        
        public PooledObject GetInvisible(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            PooledObject pooledObject = _inactiveObjects.Find(inactiveObject => !IsInstanceVisible(inactiveObject));

            return Get(pooledObject, position, rotation, parent);
        }
        
        public PooledObject GetInvisible(Transform parent)
        {
            return GetInvisible(default, default, parent);
        }

        public void CreateInactiveInstances(int quantity)
        {
            IEnumerator Routine()
            {
                for (int i = 0; i < quantity; i++)
                {
                    PooledObject pooledObject = CreateInstance(transform);
                    pooledObject.gameObject.SetActive(false);
                    pooledObject.ReleaseToPool();
                    
                    yield return null;
                }
            }

            StartCoroutine(Routine());
        }

        public void AddActiveInstance(PooledObject instance)
        {
            instance.Pool = this;

            if (!_activeObjects.Contains(instance))
            {
                _activeObjects.Add(instance);
                instance.Get();
                onObjectGot?.Invoke(instance);
            }

            if (_inactiveObjects.Contains(instance))
            {
                _inactiveObjects.Remove(instance);
            }
        }

        public void AddActiveInstance(GameObject instance)
        {
            PooledObject pooledObject = GetOrAddPooledObject(instance);
            AddActiveInstance(pooledObject);
        }

        public void AddInactiveInstance(PooledObject instance)
        {
            instance.Pool = this;

            if (_activeObjects.Contains(instance))
            {
                _activeObjects.Remove(instance);
            }
            
            if (!_inactiveObjects.Contains(instance))
            {
                _inactiveObjects.Add(instance);

                instance.ReleaseToPool();
                instance.transform.parent = transform;

                onObjectReleased?.Invoke(instance);
            }
        }

        public void AddInactiveInstance(GameObject instance)
        {
            PooledObject pooledObject = GetOrAddPooledObject(instance);
            AddInactiveInstance(pooledObject);
        }

        public void RemoveInstance(PooledObject instance)
        {
            _activeObjects.Remove(instance);
            _inactiveObjects.Remove(instance);
        }

        public PooledObject FindPooledObject(GameObject instance)
        {
            bool Predicate(PooledObject pooledObject) => pooledObject != null && pooledObject.gameObject == instance;

            PooledObject value = _activeObjects.Find(Predicate);
            if (value != null)
            {
                return value;
            }
            
            return _inactiveObjects.Find(Predicate);
        }
        
        public bool Contains(PooledObject instance)
        {
            return _activeObjects.Contains(instance) || _inactiveObjects.Contains(instance);
        }
        
        public bool Contains(GameObject instance)
        {
            return Exists(pooledObject => pooledObject.gameObject == instance);
        }

        public bool Exists(Predicate<PooledObject> match)
        {
            return _activeObjects.Exists(match) || _inactiveObjects.Exists(match);
        }
        
        public bool IsInstanceVisible(PooledObject instance)
        {
            if (!instance.gameObject.activeInHierarchy)
            {
                return false;
            }

            Renderer[] renderers = GetRenderers(instance);
            if (renderers == null || renderers.Length == 0)
            {
                return false;
            }

            return renderers.Any(renderer => renderer.isVisible);
        }

        public bool IsInstanceActive(PooledObject instance)
        {
            return _activeObjects.Contains(instance);
        }

        #endregion

        #region Private Methods

        private PooledObject Get(PooledObject instance, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            if (parent == null)
            {
                parent = transform;
            }
            
            if (instance == null)
            {
                instance = CreateInstance(position, rotation, parent);
                if (instance == null)
                {
                    return null;
                }
            }
            else
            {
                if (position != default)
                {
                    instance.transform.SetPositionAndRotation(position, rotation);
                }
                else
                {
                    instance.transform.SetPositionAndRotation(parent.position, parent.rotation);
                }

                instance.transform.SetParent(parent);
            }
            
            AddActiveInstance(instance);
            
            return instance;
        }

        private PooledObject CreateInstance(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            if (_prefab == null)
            {
                Debug.LogNull("Prefab", gameObject);
                return null;
            }

            GameObject instance;
            if (position != default)
            {
                instance = Instantiate(_prefab, position, rotation);
            }
            else if (parent != null && parent != transform)
            {
                instance = Instantiate(_prefab, parent.position, parent.rotation);
            }
            else
            {
                instance = Instantiate(_prefab);
            }

            instance.transform.SetParent(parent);

            PooledObject pooledObject = instance.GetOrAddComponent<PooledObject>();
            pooledObject.Pool = this;
            
            return pooledObject;
        }

        private PooledObject CreateInstance(Transform parent)
        {
            return CreateInstance(default, default, parent);
        }

        private PooledObject GetOrAddPooledObject(GameObject instance)
        {
            PooledObject pooledObject = FindPooledObject(instance);
            if (pooledObject == null)
            {
                pooledObject = ComponentCacheManager.GetOrAddComponent<PooledObject>(gameObject);
            }

            return pooledObject;
        }

        private Renderer[] GetRenderers(PooledObject instance)
        {
            if (_renderers.TryGetValue(instance, out Renderer[] renderers))
            {
                return renderers;
            }
            
            renderers = instance.GetComponentsInChildren<Renderer>();
            _renderers.Add(instance, renderers);

            return renderers;
        }

        private void DestroyAllInstances()
        {
            List<PooledObject> instances = new List<PooledObject>();
            instances.AddRange(_activeObjects);
            instances.AddRange(_inactiveObjects);

            foreach (PooledObject instance in instances)
            {
                if (instance != null)
                {
                    Destroy(instance);
                }
            }
            
            _activeObjects.Clear();
            _inactiveObjects.Clear();
        }

        #endregion
    }
}