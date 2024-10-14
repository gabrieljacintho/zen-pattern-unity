using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.Helpers
{
    public static class PhysicsHelper
    {
        public static bool HasObstacleBetween(Vector3 pointA, Vector3 pointB, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction,
            List<Collider> excludedColliders = null)
        {
            if (layerMask.IsEmpty())
            {
                return false;
            }
            
            Vector3 direction = (pointB - pointA).normalized;
            Ray ray = new Ray(pointA, direction);

            float maxDistance = Vector3.Distance(pointA, pointB) + UnityEngine.Physics.defaultContactOffset;
            
            RaycastHit[] hits = UnityEngine.Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);

            foreach (RaycastHit hit in hits)
            {
                if (excludedColliders == null || !excludedColliders.Contains(hit.collider))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasObstacleBetween(Vector3 originPosition, Collider targetCollider, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            if (layerMask.IsEmpty())
            {
                return false;
            }

            Vector3 targetPosition = targetCollider.AccurateClosestPoint(originPosition);
            List<Collider> excludedColliders = new() { targetCollider };
            
            return HasObstacleBetween(originPosition, targetPosition, layerMask, queryTriggerInteraction, excludedColliders);
        }

        public static bool Raycast(Vector3 originPosition, Collider targetCollider, out RaycastHit hit, LayerMask layerMask,
            QueryTriggerInteraction queryTriggerInteraction)
        {
            if (layerMask.IsEmpty())
            {
                hit = default;
                return false;
            }
            
            Vector3 targetPosition = targetCollider.AccurateClosestPoint(originPosition);
            Vector3 direction = (targetPosition - originPosition).normalized;
            Ray ray = new Ray(originPosition, direction);
            
            float maxDistance = Vector3.Distance(originPosition, targetPosition) + UnityEngine.Physics.defaultContactOffset;

            return UnityEngine.Physics.Raycast(ray, out hit, maxDistance, layerMask, queryTriggerInteraction);
        }
        
        public static bool CanDetect(GameObject targetObject, LayerMask layerMask, string requiredTag = null)
        {
            if (!layerMask.Contains(targetObject.layer))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(requiredTag) && !targetObject.CompareTag(requiredTag))
            {
                return false;
            }

            return true;
        }
    }
}