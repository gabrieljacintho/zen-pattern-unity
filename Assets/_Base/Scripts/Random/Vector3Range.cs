using System;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public class Vector3Range : Range<Vector3>
    {
        [SerializeField] private bool _keepProportion = true;


        public override Vector3 GetRandomValue()
        {
            if (_keepProportion)
            {
                float t = UnityEngine.Random.Range(0f, 1f);
                return Vector3.Lerp(MinValue, MaxValue, t);
            }

            float x = UnityEngine.Random.Range(0f, 1f);
            float y = UnityEngine.Random.Range(0f, 1f);
            float z = UnityEngine.Random.Range(0f, 1f);

            x = Mathf.Lerp(MinValue.x, MaxValue.x, x);
            y = Mathf.Lerp(MinValue.y, MaxValue.y, y);
            z = Mathf.Lerp(MinValue.z, MaxValue.x, z);

            return new Vector3(x, y, z);
        }
    }
}