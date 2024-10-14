using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class TransformSensor : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [HideIf("@_target != null")]
        [SerializeField] private string _targetId;
        [SerializeField, Min(0f)] private float _radius = 1f;
        
        public bool IsDetecting => _target != null && Vector3.Distance(transform.position, _target.position) <= _radius;
        
        [Space]
        public UnityEvent<bool> OnDetectionChanged;
        public UnityEvent OnDetect;
        public UnityEvent OnNotDetect;


        private void Awake()
        {
            if (_target == null && !string.IsNullOrEmpty(_targetId))
            {
                this.DoOnNextFrame(() =>
                {
                    _target = ComponentID.FindComponentWithID<Transform>(_targetId, true);
                    if (_target == null)
                    {
                        Debug.LogError("Target not found!", this);
                    }
                });
            }
        }

        private void LateUpdate()
        {
            Scan();
        }

        private void OnDrawGizmosSelected()
        {
            if (_radius <= 0f)
            {
                return;
            }

            Gizmos.color = IsDetecting ? Color.red : Color.white;
            
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        public void Scan()
        {
            if (IsDetecting)
            {
                OnDetectionChanged?.Invoke(true);
                OnDetect?.Invoke();
            }
            else
            {
                OnDetectionChanged?.Invoke(false);
                OnNotDetect?.Invoke();
            }
        }
    }
}