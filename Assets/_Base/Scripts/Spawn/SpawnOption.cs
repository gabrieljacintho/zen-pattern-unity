using FireRingStudio.Random;
using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Spawn
{
    [Serializable]
    public class SpawnOption : WeightedValue<GameObject>
    {
        [SerializeField] private IntReference _minQuantity = new(1);
        [SerializeField] private IntReference _maxQuantity = new(1);


        public SpawnOption(GameObject[] variants, FloatVariable weightVariable) : base(variants, weightVariable)
        {
            _minQuantity = new(1);
            _maxQuantity = new(1);
        }

        public SpawnOption(GameObject[] variants, float weight = 1) : base(variants, weight)
        {
            _minQuantity = new(1);
            _maxQuantity = new(1);
        }

        public SpawnOption(GameObject variant, FloatVariable weightVariable) : base(variant, weightVariable)
        {
            _minQuantity = new(1);
            _maxQuantity = new(1);
        }

        public SpawnOption(GameObject variant, float weight = 1) : base(variant, weight)
        {
            _minQuantity = new(1);
            _maxQuantity = new(1);
        }

        public int GetRandomQuantity()
        {
            if (_minQuantity != null && _maxQuantity != null)
            {
                return UnityEngine.Random.Range(_minQuantity.Value, _maxQuantity.Value);
            }

            if (_minQuantity != null)
            {
                return _minQuantity.Value;
            }

            if (_maxQuantity != null)
            {
                return _maxQuantity.Value;
            }

            return 1;
        }
    }
}