using System;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public class ColorRange : Range<Color>
    {
        public override Color GetRandomValue()
        {
            float t = UnityEngine.Random.Range(0f, 1f);
            return Color.Lerp(MinValue, MaxValue, t);
        }
    }
}