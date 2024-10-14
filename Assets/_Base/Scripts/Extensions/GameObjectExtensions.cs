using System.Linq;
using System.Threading;
using FireRingStudio.Cache;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
   
            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        
        public static void SetActiveChildren(this GameObject gameObject, bool value)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(value);
            }
        }
        
        public static GameObject FindChildWithName(this GameObject gameObject, string name)
        {
            if (gameObject.name == name)
                return gameObject;
            
            foreach (Transform child in gameObject.transform)
            {
                GameObject result = child.gameObject.FindChildWithName(name);
                if (result != null)
                    return result;
            }

            return null;
        }
        
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent(out T t))
            {
                t = gameObject.AddComponent<T>();
            }
            
            return t;
        }

        public static Component GetOrAddComponent(this GameObject gameObject, System.Type type)
        {
            if (!gameObject.TryGetComponent(type, out Component component))
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        public static T[] GetInterfacesInChildren<T>(this GameObject gameObject, bool includeInactive = false)
        {
            return gameObject.GetComponentsInChildren<MonoBehaviour>(includeInactive).OfType<T>().ToArray();
        }

        public static PooledObject Get(this GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            ObjectPool pool = PoolManager.GetPool(prefab.name, prefab);
            return pool.Get(position, rotation, parent);
        }

        public static PooledObject Get(this GameObject prefab, Transform parent = null)
        {
            ObjectPool pool = PoolManager.GetPool(prefab.name, prefab);
            return pool.Get(parent);
        }
        
        public static PooledObject GetInvisible(this GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            ObjectPool pool = PoolManager.GetPool(prefab.name, prefab);
            return pool.GetInvisible(position, rotation, parent);
        }
        
        public static PooledObject GetInvisible(this GameObject prefab, Transform parent = null)
        {
            ObjectPool pool = PoolManager.GetPool(prefab.name, prefab);
            return pool.GetInvisible(parent);
        }
        
        public static void ReleaseToPool(this GameObject gameObject, string poolTag, float delay = 0f, bool inRealtime = false)
        {
            ObjectPool pool = PoolManager.FindPoolWithInstance(gameObject);

            if (pool == null && !string.IsNullOrEmpty(poolTag))
            {
                pool = PoolManager.FindPoolWithTag(poolTag);
            }

            PooledObject pooledObject = null;
            if (pool != null)
            {
                pooledObject = pool.FindPooledObject(gameObject);
            }

            if (pooledObject == null)
            {
                pooledObject = ComponentCacheManager.GetOrAddComponent<PooledObject>(gameObject);
            }

            if (pool != null)
            {
                pooledObject.Pool = pool;
            }

            pooledObject.ReleaseToPool(delay, inRealtime);
        }

        public static void ReleaseToPool(this GameObject gameObject, float delay = 0f, bool inRealtime = false)
        {
            ReleaseToPool(gameObject, string.Empty, delay, inRealtime);
        }

        public static GameObject FindChildWithID(this GameObject gameObject, string id, bool includeInactive = false)
        {
            return GameObjectID.FindChildWithID(gameObject, id, includeInactive);
        }
    }
}