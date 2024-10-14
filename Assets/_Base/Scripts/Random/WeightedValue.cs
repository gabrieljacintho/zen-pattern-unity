using System;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public class WeightedValue<T>
    {
        [SerializeField] private T[] _variants;
        [SerializeField] private FloatVariable _weightVariable;
        [HideIf("@_weightVariable != null")]
        [SerializeField, Range(0f, 1f)] private float _weight = 1f;

        public T[] Variants => _variants;
        public T FirstVariant => _variants != null && _variants.Length > 0 ? _variants[0] : default;

        public FloatVariable WeightVariable => _weightVariable;
        public float Weight => _weightVariable != null ? _weightVariable.Value : _weight;

        
        public WeightedValue(T[] variants, FloatVariable weightVariable)
        {
            _variants = variants;
            _weightVariable = weightVariable;
        }
        
        public WeightedValue(T[] variants, float weight = 1f)
        {
            _variants = variants;
            _weight = weight;
        }
        
        public WeightedValue(T variant, FloatVariable weightVariable)
        {
            _variants = new [] { variant };
            _weightVariable = weightVariable;
        }
        
        public WeightedValue(T variant, float weight = 1f)
        {
            _variants = new [] { variant };
            _weight = weight;
        }

        public T GetRandomVariant()
        {
            if (_variants == null || _variants.Length == 0)
                return default;

            int index = UnityEngine.Random.Range(0, _variants.Length);
            return _variants[index];
        }

        public void ForEachVariant(Action<T> action)
        {
            if (_variants == null)
            {
                return;
            }

            foreach (T variant in _variants)
            {
                if (variant != null)
                {
                    action?.Invoke(variant);
                }
            }
        }
    }
}
