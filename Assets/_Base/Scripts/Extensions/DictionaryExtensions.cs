using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Shuffle<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            dictionary.OrderBy(x => UnityEngine.Random.Range(0, Mathf.Infinity))
                .ToDictionary(item => item.Key, item => item.Value);
        }
    }
}