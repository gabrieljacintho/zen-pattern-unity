using FireRingStudio.Assets._AssetBase.Scripts;
using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(Rigidbody))]
    public class AttachRigidbody : PooledBehaviour
    {
        private Rigidbody _rigidbody;
        private Collider _targetCollider;

        public Collider TargetCollider
        {
            get => _targetCollider;
            set
            {
                if (_targetCollider != value)
                {
                    _targetCollider = value;
                    UpdateRigidbody();
                }
            }
        }


        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            UpdateRigidbody();
        }

        private void UpdateRigidbody()
        {
            if (_targetCollider == null)
            {
                return;
            }

            if (CanAttach(_targetCollider))
            {
                _rigidbody.isKinematic = true;
            }
            else
            {
                _rigidbody.isKinematic = false;
                _targetCollider = null;
            }
        }

        protected override void OnRelease()
        {
            TargetCollider = null;
        }

        private bool CanAttach(Collider collider)
        {
            return collider.enabled && collider.gameObject.activeInHierarchy;
        }
    }
}