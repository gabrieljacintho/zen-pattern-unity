using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
        
        public static bool TryGetValue<T>(this List<T> list, Predicate<T> match, out T value)
        {
            value = list.Find(match);
            
            return value != null;
        }
        
        public static T Dequeue<T>(this List<T> list)
        {
            T value = list[0];
            list.RemoveAt(0);
            
            return value;
        }

        public static T FirstOrDefault<T>(this List<T> list)
        {
            return list.Count > 0 ? list[0] : default;
        }

        public static void AddRangeUncontained<T>(this List<T> list, List<T> range)
        {
            foreach (T item in range)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        #region Dictionary

        public static bool TryGetValue<TKey, TValue>(this List<KeyValue<TKey, TValue>> list, TKey key, out TValue value)
        {
            bool Predicate(KeyValue<TKey, TValue> keyValue)
            {
                return key != null ? keyValue.Key != null && keyValue.Key.Equals(key) : keyValue.Key == null;
            }

            if (list.Exists(Predicate))
            {
                value = list.Find(Predicate).Value;
                return true;
            }

            value = default;
            return false;
        }
        
        public static bool TryGetValues<TKey, TValue>(this List<KeyValue<TKey, TValue>> list, TKey key, out List<TValue> values)
        {
            bool Predicate(KeyValue<TKey, TValue> keyValue)
            {
                return key != null ? keyValue.Key != null && keyValue.Key.Equals(key) : keyValue.Key == null;
            }

            if (list.Exists(Predicate))
            {
                List<KeyValue<TKey, TValue>> keyValues = list.FindAll(keyValue => keyValue.Key.Equals(key));
                values = keyValues.Select(keyValue => keyValue.Value);
                return true;
            }

            values = null;
            return false;
        }

        public static List<TResult> Select<TSource, TResult>(this List<TSource> list, Func<TSource,TResult> selector)
        {
            List<TResult> results = new List<TResult>();
            foreach (TSource item in list)
            {
                TResult result = selector.Invoke(item);
                results.Add(result);
            }

            return results;
        }
        
        public static T FindValueWithID<T>(this List<KeyValue<string, T>> list, string id)
        {
            KeyValue<string, T> idValue;
            if (string.IsNullOrEmpty(id))
            {
                idValue = list.Find(x => string.IsNullOrEmpty(x.Key));
            }
            else
            {
                idValue = list.Find(x => x.Key == id);
            }

            return idValue.Value;
        }
        
        public static List<TK> GetKeys<TK, TV>(this List<KeyValue<TK, TV>> list)
        {
            return list.Select(keyValue => keyValue.Key);
        }

        public static List<TV> GetValues<TK, TV>(this List<KeyValue<TK, TV>> list)
        {
            return list.Select(keyValue => keyValue.Value);
        }
        
        public static bool ContainsKey<TK, TV>(this List<KeyValue<TK, TV>> list, TK key)
        {
            return list.Exists(keyValue => keyValue.Key.Equals(key));
        }
        
        public static bool ContainsValue<TK, TV>(this List<KeyValue<TK, TV>> list, TV value)
        {
            return list.Exists(keyValue => keyValue.Value.Equals(value));
        }
        
        #endregion
    }
}
