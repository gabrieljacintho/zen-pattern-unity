using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Pool
{
    public static class PoolManager
    {
        private static readonly List<ObjectPool> s_pools = new();

        private static GameObject s_poolsRoot;

        private static GameObject PoolsRoot
        {
            get
            {
                if (s_poolsRoot == null)
                {
                    s_poolsRoot = new GameObject("Pools");
                    Object.DontDestroyOnLoad(s_poolsRoot);
                }

                return s_poolsRoot;
            }
        }
        

        public static void AddPool(ObjectPool pool)
        {
            if (s_pools.Contains(pool))
            {
                return;
            }
            
            s_pools.Add(pool);
            pool.transform.SetParent(PoolsRoot.transform);
        }

        public static void RemovePool(ObjectPool pool)
        {
            s_pools.Remove(pool);
        }

        public static ObjectPool GetPool(string poolTag, GameObject prefab, int initialSize = 0)
        {
            ObjectPool pool = FindPoolWithTag(poolTag);
            if (pool != null)
            {
                pool.Prefab = prefab;
                return pool;
            }
            
            GameObject poolObject = new GameObject("Pool (" + poolTag + ")");

            pool = poolObject.AddComponent<ObjectPool>();
            pool.Initialize(poolTag, prefab, initialSize);
            
            AddPool(pool);

            return pool;
        }
        
        public static PooledObject Get(string poolTag, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            ObjectPool pool = FindValidPoolWithTag(poolTag);
            if (pool == null)
            {
                Debug.LogError("Valid pool with tag \"" + poolTag + "\" not found!");
                return null;
            }

            return pool.Get(position, rotation, parent);
        }

        public static PooledObject Get(string poolTag, Transform parent)
        {
            Vector3 position = parent != null ? parent.position : default;
            Quaternion rotation = parent != null ? parent.rotation : default;
            
            return Get(poolTag, position, rotation, parent);
        }

        public static PooledObject GetInvisible(string poolTag, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            ObjectPool pool = FindValidPoolWithTag(poolTag);
            if (pool == null)
            {
                Debug.LogError("Valid pool with tag \"" + poolTag + "\" not found!");
                return null;
            }

            return pool.GetInvisible(position, rotation, parent);
        }

        public static PooledObject GetInvisible(string poolTag, Transform parent = null)
        {
            Vector3 position = parent != null ? parent.position : default;
            Quaternion rotation = parent != null ? parent.rotation : default;
            
            return GetInvisible(poolTag, position, rotation, parent);
        }

        public static ObjectPool FindPoolWithTag(string poolTag)
        {
            return s_pools.Find(pool => pool.Tag == poolTag);
        }

        public static ObjectPool FindPoolWithInstance(GameObject instance)
        {
            return s_pools.Find(pool => pool.Contains(instance));
        }

        public static ObjectPool FindValidPoolWithTag(string poolTag)
        {
            return s_pools.Find(pool => pool.Tag == poolTag && pool.Prefab != null);
        }
        
        public static bool HasValidPoolWithTag(string poolTag)
        {
            return FindValidPoolWithTag(poolTag) != null;
        }
    }
}