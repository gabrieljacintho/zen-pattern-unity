using FireRingStudio.Helpers;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Physics
{
    [RequireComponent(typeof(Collider))]
    public abstract class HitArea : MonoBehaviour
    {
        [Tooltip("Set 0 to only hit on enter.")]
        [SerializeField] private IntReference _hitsPerMinuteReference = new(0);
        
        private float _lastHitTime;
        
        protected abstract LayerMask LayerMask { get; }
        protected abstract QueryTriggerInteraction QueryTriggerInteraction { get; }
        private int HitsPerMinute => _hitsPerMinuteReference?.Value ?? 0;

        [Space]
        [InspectorName("On Hit")] public UnityEvent<RaycastHit> OnHitEvent;


        protected virtual void Start()
        {
            // Show enabled toggle in inspector
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            TryHit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isActiveAndEnabled || HitsPerMinute < 1 || Time.time < _lastHitTime + 1f / HitsPerMinute)
            {
                return;
            }

            TryHit(other);
        }

        private void TryHit(Collider other)
        {
            if (!PhysicsHelper.CanDetect(other.gameObject, LayerMask))
            {
                return;
            }

            if (!PhysicsHelper.Raycast(transform.position, other, out RaycastHit hit, LayerMask, QueryTriggerInteraction))
            {
                return;
            }
            
            OnHit(other, hit);

            OnHitEvent?.Invoke(hit);
            
            _lastHitTime = Time.time;
        }

        protected abstract void OnHit(Collider other, RaycastHit hit);
    }
}