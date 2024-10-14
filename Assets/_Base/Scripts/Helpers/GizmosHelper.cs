using UnityEditor;
using UnityEngine;

namespace FireRingStudio.Helpers
{
    public static class GizmosHelper
    {
        public static void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius, Color color = default)
        {
#if UNITY_EDITOR
            if (color != default)
            {
                Handles.color = color;
            }
 
            Vector3 forward = point2 - point1;
            float length = forward.magnitude;
            Quaternion rotation = length > 0f ? Quaternion.LookRotation(forward) : Quaternion.identity;
            Vector3 center2 = new Vector3(0f,0,length);
            float pointOffset = radius / 2f;
       
            Matrix4x4 angleMatrix = Matrix4x4.TRS(point1, rotation, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);
           
                
                Handles.DrawLine(new Vector3(radius, 0f, 0f), new Vector3(radius, 0f, length));
                Handles.DrawLine(new Vector3(-radius, 0f, 0f), new Vector3(-radius, 0f, length));
                Handles.DrawLine(new Vector3(0f, radius, 0f), new Vector3(0f, radius, length));
                Handles.DrawLine(new Vector3(0f, -radius, 0f), new Vector3(0f, -radius, length));
            }
#endif
        }

        public static void DrawAngle(Vector3 position, Vector3 eulerAngles, float angle, float distance = 2f)
        {
            Vector3 viewAngleL = DirectionFromAngle(eulerAngles.y, -angle / 2);
            Vector3 viewAngleR = DirectionFromAngle(eulerAngles.y, angle / 2);

            Gizmos.DrawLine(position, position + viewAngleL * distance);
            Gizmos.DrawLine(position, position + viewAngleR * distance);
        }

        public static void DrawAngle(Transform transform, float angle, float distance = 2f)
        {
            DrawAngle(transform.position, transform.eulerAngles, angle, distance);
        }

        private static Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            float x = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
            float z = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }
    }
}