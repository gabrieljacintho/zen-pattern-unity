using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Cache
{
    public static class ComponentCacheManager
    {
        private static readonly List<ComponentCache> s_componentCaches = new List<ComponentCache>();


        public static T GetComponent<T>(GameObject gameObject)
        {
            ComponentCache componentCache = GetComponentCacheOfType<T>();
            return componentCache.GetComponent<T>(gameObject);
        }

        public static T GetOrAddComponent<T>(GameObject gameObject)
        {
            ComponentCache componentCache = GetComponentCacheOfType<T>();
            return componentCache.GetOrAddComponent<T>(gameObject);
        }

        private static ComponentCache GetComponentCacheOfType<T>()
        {
            Type type = typeof(T);
            ComponentCache componentCache = s_componentCaches.Find(cache => cache.Type == type);
            if (componentCache == null)
            {
                componentCache = new ComponentCache(type);
                s_componentCaches.Add(componentCache);
            }

            return componentCache;
        }
    }
}