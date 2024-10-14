using System;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.Patterns
{
    public abstract class NonSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T CurrentInstance { get; protected set; }
        public static List<T> LastInstances { get; protected set; } = new List<T>();
        
        public static Action<T> InstanceChanged;


        protected virtual void OnEnable()
        {
            if (CurrentInstance != null)
            {
                LastInstances.Add(CurrentInstance);
            }
            
            CurrentInstance = this as T;
            
            InstanceChanged?.Invoke(CurrentInstance);
        }

        protected virtual void OnDisable()
        {
            if (CurrentInstance == this)
            {
                CurrentInstance = LastInstances.Count > 0 ? LastInstances.Dequeue() : null;
                InstanceChanged?.Invoke(CurrentInstance);
            }
            else if (LastInstances.Contains(this as T))
            {
                LastInstances.Remove(this as T);
            }
        }
    }
}