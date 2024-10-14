using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class ColliderExtensions
    {
        public static bool IsInside(this Collider collider, Vector3 point)
        {
            return collider.bounds.Contains(point);
        }
        
        public static Vector3 NearestVertexTo(this MeshCollider meshCollider, Vector3 position)
        {
            position = meshCollider.transform.InverseTransformPoint(position);

            float minDistanceSqr = Mathf.Infinity;
            Vector3 nearestVertex = Vector3.zero;
            
            foreach (Vector3 vertex in meshCollider.sharedMesh.vertices)
            {
                Vector3 diff = position - vertex;
                float distanceSqr = diff.sqrMagnitude;
                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearestVertex = vertex;
                }
            }
            
            return meshCollider.transform.TransformPoint(nearestVertex);
        }

        public static Vector3 AccurateClosestPoint(this Collider collider, Vector3 position)
        {
            if (collider is MeshCollider meshCollider && !meshCollider.convex)
            {
                return meshCollider.NearestVertexTo(position);
            }
            
            return collider.ClosestPoint(position);
        }
    }
}