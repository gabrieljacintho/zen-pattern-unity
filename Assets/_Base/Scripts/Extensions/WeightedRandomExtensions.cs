using System.Collections.Generic;
using FireRingStudio.Random;
using FireRingStudio.Spawn;
using Sirenix.Utilities;
using Subtegral.WeightedRandom;

namespace FireRingStudio.Extensions
{
    public static class WeightedRandomExtensions
    {
        public static void Add<T>(this WeightedRandom<WeightedValue<T>> weightedRandom, WeightedValue<T> value)
        {
            weightedRandom.Add(value, value.Weight);
        }
        
        public static void AddRange<T>(this WeightedRandom<WeightedValue<T>> weightedRandom, IEnumerable<WeightedValue<T>> values)
        {
            values.ForEach(x => weightedRandom.Add(x, x.Weight));
        }

        public static void Add(this WeightedRandom<SpawnOption> weightedRandom, SpawnOption value)
        {
            weightedRandom.Add(value, value.Weight);
        }

        public static void AddRange(this WeightedRandom<SpawnOption> weightedRandom, IEnumerable<SpawnOption> values)
        {
            values.ForEach(x => weightedRandom.Add(x, x.Weight));
        }
    }
}