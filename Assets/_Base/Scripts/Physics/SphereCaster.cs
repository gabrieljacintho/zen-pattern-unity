using UnityEngine;

namespace FireRingStudio.Physics
{
    public class SphereCaster : Caster
    {
        [Header("Sphere")]
        [SerializeField, Min(0f)] private float _radius = 1f;
        [SerializeField, Min(0f)] private float _maxDistance = 1f;
        
        
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            
            Vector3 offset = transform.forward * _maxDistance;
            Vector3 center = transform.position + offset;
            
            Gizmos.color = IsDetecting ? Color.red : Color.white;
            Gizmos.DrawWireSphere(center, _radius);
        }
        
        protected override int Cast(out RaycastHit[] results, int layerMask = ~0)
        {
            Ray ray = new Ray(transform.position, transform.forward);

            if (_useNonAlloc)
            {
                results = new RaycastHit[_maxAmountOfHits];
                return UnityEngine.Physics.SphereCastNonAlloc(ray, _radius, results, _maxDistance, layerMask, _triggerInteraction);
            }
            
            results = UnityEngine.Physics.SphereCastAll(ray, _radius, _maxDistance, layerMask, _triggerInteraction);
            return results != null ? results.Length : 0;
        }
    }
}