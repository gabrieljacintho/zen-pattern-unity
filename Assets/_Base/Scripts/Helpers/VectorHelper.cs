using UnityEngine;

namespace FireRingStudio.Helpers
{
    public class VectorHelper : MonoBehaviour
    {
        public static Vector3 Random(Vector3 range)
        {
            Vector3 value = new Vector3(
                UnityEngine.Random.Range(-range.x, range.x),
                UnityEngine.Random.Range(-range.y, range.y),
                UnityEngine.Random.Range(-range.z, range.z)
            );
            
            return value;
        }

        public static Vector3 Distance(Vector3 a, Vector3 b)
        {
            Vector3 value = a - b;
            value.x = Mathf.Abs(value.x);
            value.y = Mathf.Abs(value.y);
            value.z = Mathf.Abs(value.z);

            return value;
        }

        public static Vector2Int RoundToVector2Int(Vector2 vector2)
        {
            int x = Mathf.RoundToInt(vector2.x);
            int y = Mathf.RoundToInt(vector2.y);
            
            return new Vector2Int(x, y);
        }
    }
}