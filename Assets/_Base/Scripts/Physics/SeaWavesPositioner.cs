using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Physics
{
    public class SeaWavesPositioner : MonoBehaviour
    {
        [SerializeField] private float _radius = 20f;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction;
        [SerializeField] private float _minHeight = 50f;
        [SerializeField] private float _raycastSpacing = 0.1f;
        [SerializeField] private float _targetDistance = 10f;


        [Button]
        public void UpdatePosition()
        {
            if (!TryGetClosestPoint(out Vector3 closestPoint))
            {
                Debug.LogWarning("New position not found!", this);
                return;
            }

            Vector3 direction = (transform.position - closestPoint).normalized;

            Vector3 newPosition = direction * _targetDistance;
            newPosition.y = transform.position.y;

            transform.position = newPosition;
        }

        private bool TryGetClosestPoint(out Vector3 closestPoint)
        {
            List<Vector3> hitPoints = GetHitPoints();

            closestPoint = Vector3.zero;
            bool hasClosestHitPoint = false;
            foreach (Vector3 hitPoint in hitPoints)
            {
                if (hitPoint.y < _minHeight)
                {
                    continue;
                }

                hasClosestHitPoint = true;

                if (!hasClosestHitPoint)
                {
                    closestPoint = hitPoint;
                    continue;
                }

                float distanceA = Vector3.Distance(transform.position, closestPoint);
                float distanceB = Vector3.Distance(transform.position, hitPoint);
                if (distanceB < distanceA)
                {
                    closestPoint = hitPoint;
                }
            }

            return hasClosestHitPoint;
        }

        private List<Vector3> GetHitPoints()
        {
            List<Vector3> hitPoints = new List<Vector3>();

            int i = Mathf.RoundToInt(_radius * 2f / _raycastSpacing);

            Vector3 position = transform.position;
            position.x -= _radius;
            position.z -= _radius;

            Vector3 direction = transform.TransformDirection(_direction);

            for (int x = 0; x < i; x++)
            {
                for (int z = 0; z < i; z++)
                {
                    if (UnityEngine.Physics.Raycast(position, direction, out RaycastHit hit, Mathf.Infinity, _layerMask, _queryTriggerInteraction))
                    {
                        hitPoints.Add(hit.point);
                    }

                    position.z += _raycastSpacing;
                }
                position.x += _raycastSpacing;
            }

            return hitPoints;
        }
    }
}
