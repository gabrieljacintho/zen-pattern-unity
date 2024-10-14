using System;

namespace FireRingStudio
{
    [Serializable]
    public struct KeyValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}