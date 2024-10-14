using System;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public abstract class Range<T>
    {
        [SerializeField] private T _minValue;
        [SerializeField] private T _maxValue;

        public T MinValue => _minValue;
        public T MaxValue => _maxValue;


        public Range()
        {
            
        }

        public Range(T minValue, T maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public abstract T GetRandomValue();
    }
}