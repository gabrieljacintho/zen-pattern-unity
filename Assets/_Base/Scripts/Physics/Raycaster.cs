using UnityEngine;

namespace FireRingStudio.Physics
{
    public class Raycaster : Caster
    {
        [Header("Ray")]
        [SerializeField] private Vector3 _direction = Vector3.forward;
        [SerializeField, Min(0f)] private float _maxDistance = 1f;

        
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            
            Vector3 point1 = transform.position;

            Vector3 direction = transform.TransformDirection(_direction);
            Vector3 point2 = point1 + direction * _maxDistance;      
            
            Gizmos.color = IsDetecting ? Color.red : Color.white;
            Gizmos.DrawLine(point1, point2);
        }

        protected override int Cast(out RaycastHit[] results, int layerMask = ~0)
        {
            Vector3 originPosition = transform.position;
            Vector3 direction = transform.TransformDirection(_direction);
            Ray ray = new Ray(originPosition, direction);

            if (_useNonAlloc)
            {
                results = new RaycastHit[_maxAmountOfHits];
                return UnityEngine.Physics.RaycastNonAlloc(ray, results, _maxDistance, layerMask, _triggerInteraction);
            }
            
            results = UnityEngine.Physics.RaycastAll(ray, _maxDistance, layerMask, _triggerInteraction);
            return results != null ? results.Length : 0;
        }
    }
}