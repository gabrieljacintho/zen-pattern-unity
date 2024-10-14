using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.Physics
{
    public class CapsuleCaster : Caster
    {
        [Header("Capsule")]
        [SerializeField, Min(0f)] private float _radius = 1f;
        [SerializeField, Min(0f)] private float _length = 2f;
        [SerializeField, Min(0f)] private float _maxDistance;

        
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Vector3 direction = transform.forward;
            Vector3 offset = direction * _maxDistance;
            
            Vector3 point1 = transform.position + offset;
            Vector3 point2 = point1 + direction * _length + offset;
            
            Color color = IsDetecting ? Color.red : Color.white;
            GizmosHelper.DrawWireCapsule(point1, point2, _radius, color);
        }

        protected override int Cast(out RaycastHit[] results, int layerMask = ~0)
        {
            Vector3 point1 = transform.position;
            Vector3 direction = transform.forward;
            Vector3 point2 = point1 + direction * _length;
            
            if (_useNonAlloc)
            {
                results = new RaycastHit[_maxAmountOfHits];
                return UnityEngine.Physics.CapsuleCastNonAlloc(point1, point2, _radius, direction, results, _maxDistance, layerMask, _triggerInteraction);
            }
            
            results = UnityEngine.Physics.CapsuleCastAll(point1, point2, _radius, direction, _maxDistance, layerMask, _triggerInteraction);
            return results != null ? results.Length : 0;
        }
    }
}