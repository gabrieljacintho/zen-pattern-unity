using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio
{
    public class ComponentID : MonoBehaviour
    {
        [SerializeField] private Component _component;
        [SerializeField] private string _id;
        
        private static readonly List<ComponentID> s_componentIDs = new();

        public Component Component => _component;
        public string ID => _id;
        
        public static Action<Component> ComponentRegistered;
        public static Action Updated;
        
        
        private void Awake()
        {
            Register();
        }
        
        private void OnDestroy()
        {
            Unregister();
        }
        
        public static T FindComponentWithID<T>(string id, bool includeInactive = false) where T : Component
        {
            List<T> components = FindComponentsWithID<T>(id);
            if (components.Count == 0 && includeInactive)
            {
                components = FindComponentsWithID<T>(id, true);
            }

            return components.Count > 0 ? components[0] : null;
        }

        public static List<T> FindComponentsWithID<T>(string id, bool includeInactive = false) where T : Component
        {
            if (includeInactive)
            {
                RegisterAllComponents();
            }

            List<T> components = new List<T>();
            foreach (ComponentID componentID in s_componentIDs)
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (!string.IsNullOrEmpty(componentID.ID))
                    {
                        continue;
                    }
                }
                else if (componentID.ID != id)
                {
                    continue;
                }
                
                if (!includeInactive && !componentID.isActiveAndEnabled)
                {
                    continue;
                }

                if (componentID.Component is T component)
                {
                    components.Add(component);
                }
            }
            
            return components;
        }
        
        private void Register()
        {
            if (s_componentIDs.Contains(this))
            {
                return;
            }
            
            s_componentIDs.Add(this);

            if (_component != null)
            {
                ComponentRegistered?.Invoke(_component);
            }
            
            Updated?.Invoke();
        }

        private void Unregister()
        {
            if (!s_componentIDs.Contains(this))
            {
                return;
            }
            
            s_componentIDs.Remove(this);
                
            Updated?.Invoke();
        }

        private static void RegisterAllComponents()
        {
            ComponentID[] componentIDs = FindObjectsOfType<ComponentID>(true);
            Array.ForEach(componentIDs, componentID => componentID.Register());
        }
    }
}