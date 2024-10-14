using System.Collections.Generic;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using Subtegral.WeightedRandom;
using UnityEngine;

namespace FireRingStudio.Random
{
    public abstract class Randomizer<T> : MonoBehaviour
    {
        [SerializeField] private List<WeightedValue<T>> _values;
        [SerializeField] private bool _randomOnAwake;
        [SerializeField] private bool _randomOnEnable;

        private WeightedRandom<WeightedValue<T>> _random;


        protected virtual void Awake()
        {
            LoadValues();
            
            if (_randomOnAwake)
            {
                Random();
            }
        }

        protected virtual void OnEnable()
        {
            if (_randomOnEnable)
            {
                Random();
            }
        }

        [Button]
        public void Random()
        {
            if (_random == null)
            {
                LoadValues();
            }

            WeightedValue<T> randomValue = _random.Next();

            ApplyValue(randomValue.GetRandomVariant());
        }

        protected abstract void ApplyValue(T value);

        private void LoadValues()
        {
            _random = null;
            
            if (_values == null)
            {
                return;
            }

            foreach (WeightedValue<T> value in _values)
            {
                if (value.Variants == null || value.Variants.All(x => x == null))
                {
                    continue;
                }

                if (_random == null)
                {
                    _random = new WeightedRandom<WeightedValue<T>>();
                }

                _random.Add(value, value.Weight);
            }
        }
    }
}