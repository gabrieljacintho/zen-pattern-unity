using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(Rigidbody))]
    public class AttachedRigidbody : MonoBehaviour
    {
        [SerializeField] private bool _isKinematic;
        [SerializeField] private Collider _attachedCollider;

        private Rigidbody _rigidbody;

        public Collider AttachedCollider
        {
            get => _attachedCollider;
            set => _attachedCollider = value;
        }


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            _rigidbody.isKinematic = _isKinematic && (_attachedCollider == null || _attachedCollider.enabled);
        }
    }
}