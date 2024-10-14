using FireRingStudio.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Cache
{
    public class ComponentCache
    {
        private readonly Type _type;
        private readonly Dictionary<GameObject, object> _componentByObject;

        public Type Type => _type;


        public ComponentCache(Type type)
        {
            _type = type;
            _componentByObject = new Dictionary<GameObject, object>();
        }
        
        public T GetComponent<T>(GameObject gameObject)
        {
            if (_componentByObject.TryGetValue(gameObject, out object component))
            {
                return (T)component;
            }

            component = gameObject.GetComponent(_type);
            _componentByObject.TryAdd(gameObject, component);

            return (T)component;
        }

        public T GetOrAddComponent<T>(GameObject gameObject)
        {
            if (_componentByObject.TryGetValue(gameObject, out object component))
            {
                return (T)component;
            }

            component = gameObject.GetOrAddComponent(_type);
            _componentByObject.TryAdd(gameObject, component);

            return (T)component;
        }
    }
}