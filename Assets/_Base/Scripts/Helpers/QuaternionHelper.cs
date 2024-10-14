using UnityEngine;

namespace FireRingStudio.Helpers
{
    public static class QuaternionHelper
    {
        public static Quaternion Random(Vector3 range)
        {
            Vector3 value = VectorHelper.Random(range);
            
            return Quaternion.Euler(value);
        }
    }
}