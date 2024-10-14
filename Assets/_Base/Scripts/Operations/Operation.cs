using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;

namespace FireRingStudio.Operations
{
    [Serializable]
    public abstract class Operation<T>
    {
        public T DefaultValue;
        public List<AtomBaseVariable<T>> Atoms;

        private Dictionary<Component, T> _valueByComponent;

        [ShowInInspector]
        public T Result => GetResult();
        private Dictionary<Component, T> ValueByComponent
        {
            get
            {
                if (_valueByComponent == null)
                {
                    _valueByComponent = new Dictionary<Component, T>();
                }

                return _valueByComponent;
            }
        }


        public Operation(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public void SetComponentValue(Component component, T value)
        {
            ValueByComponent[component] = value;
        }

        public void RemoveComponent(Component component)
        {
            ValueByComponent.Remove(component);
        }

        public void ClearComponentValues()
        {
            ValueByComponent.Clear();
        }

        private T GetResult()
        {
            T value = DefaultValue;
            
            if (Atoms != null)
            {
                foreach (AtomBaseVariable<T> valueReference in Atoms)
                {
                    if (valueReference != null)
                    {
                        value = GetResult(value, valueReference.Value);
                    }
                }
            }

            foreach (T otherValue in ValueByComponent.Values)
            {
                value = GetResult(value, otherValue);
            }

            return value;
        }

        protected abstract T GetResult(T valueA, T valueB);
    }
}