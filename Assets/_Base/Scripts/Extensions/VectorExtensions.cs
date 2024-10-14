using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class VectorExtensions
    {
        public static bool IsVisibleByCamera(this Vector3 position, Camera camera)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(position);
            return screenPoint.z > 0f && screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f;
        }
        
        public static bool IsVisibleByMainCamera(this Vector3 position)
        {
            Camera mainCamera = Camera.main;
            return mainCamera != null && position.IsVisibleByCamera(mainCamera);
        }

        public static bool IsVisibleByCurrentCamera(this Vector3 position)
        {
            Camera currentCamera = Camera.current;
            return currentCamera != null && position.IsVisibleByCamera(currentCamera);
        }
        
        public static Vector3 GetClosestPosition(this Vector3 originPosition, Vector3 aPosition, Vector3 bPosition)
        {
            float aDistance = Vector3.Distance(originPosition, aPosition);
            float bDistance = Vector3.Distance(originPosition, bPosition);

            return aDistance < bDistance ? aPosition : bPosition;
        }

        public static bool CheckAngle(this Vector3 originPosition, Vector3 targetPosition, Vector3 forward, float angle)
        {
            if (angle <= 0f)
            {
                return false;
            }

            if (angle >= 360f)
            {
                return true;
            }

            Vector3 direction = (targetPosition - originPosition).normalized;

            if (Vector3.Angle(forward, direction) > angle / 2f)
            {
                return false;
            }

            return true;
        }

        public static float RandomValue(this Vector2 vector2)
        {
            return UnityEngine.Random.Range(vector2.x, vector2.y);
        }
    }
}
