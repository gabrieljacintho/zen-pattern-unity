using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Physics
{
    public abstract class Detector : DetectorBase
    {
        public bool IsDetecting => _colliders.Count > 0;
        
        protected List<Collider> _colliders = new List<Collider>();
        
        [Space]
        [FormerlySerializedAs("onChangeDetectionState")]
        public UnityEvent<bool> OnChangeDetectionState;
        [FormerlySerializedAs("onDetect")]
        public UnityEvent OnDetect;
        [FormerlySerializedAs("onNotDetect")]
        public UnityEvent OnNotDetect;
        [SerializeField] protected bool _canInvokeOnDisable = true;
        [SerializeField] protected bool _alwaysInvoke;


        protected virtual void OnDisable()
        {
            if (IsDetecting)
            {
                _colliders.Clear();

                if (_canInvokeOnDisable)
                {
                    OnNotDetect?.Invoke();
                }
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = IsDetecting ? Color.red : Color.white;
            
            foreach (Collider other in _colliders)
            {
                Gizmos.DrawLine(transform.position, other.transform.position);
            }
        }
        
        public List<Collider> GetCollidersOrderByDistance()
        {
            _colliders.RemoveAll(collider => collider == null);
            return _colliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToList();
        }

        protected void TryInvokeEvents(bool wasDetecting)
        {
            if (IsDetecting)
            {
                if (!wasDetecting || _alwaysInvoke)
                {
                    OnChangeDetectionState?.Invoke(true);
                    OnDetect?.Invoke();
                }
            }
            else if (wasDetecting || _alwaysInvoke)
            {
                OnChangeDetectionState?.Invoke(false);
                OnNotDetect?.Invoke();
            }
        }
    }
}