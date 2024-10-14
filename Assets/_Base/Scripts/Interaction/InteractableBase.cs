using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Interaction
{
    public abstract class InteractableBase : MonoBehaviour
    {
        [SerializeField, ES3NonSerializable] protected bool _interactable = true;
        [SerializeField, ES3NonSerializable] protected bool _inspectable;
        [ShowIf("_inspectable")]
        [SerializeField, ES3NonSerializable] protected float _moveSpeed = 1f;
        [ShowIf("_inspectable")]
        [SerializeField, ES3NonSerializable] protected float _rotateSpeed = 360f;

        private Vector3 _originPosition;
        private Quaternion _originRotation;

        private Transform _inspectPoint;
        private bool _isBeingInspected;

        public virtual bool Interactable
        {
            get => _interactable;
            set => SetInteractable(value);
        }
        public virtual bool Inspectable
        {
            get => _inspectable;
            set => _inspectable = value;
        }
        public virtual string Description { get; }


        protected virtual void Awake()
        {
            _originPosition = transform.position;
            _originRotation = transform.rotation;
        }

        protected virtual void Update()
        {
            if (GameManager.IsPaused || _inspectPoint == null)
            {
                return;
            }

            Vector3 targetPositon = _isBeingInspected ? _inspectPoint.position : _originPosition;
            transform.position = Vector3.MoveTowards(transform.position, targetPositon, _moveSpeed * Time.deltaTime);

            Quaternion targetRotation = _isBeingInspected ? _inspectPoint.rotation : _originRotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
        }

        public void SetInteractable(bool value)
        {
            _interactable = value;
        }

        public void StartInspect(Transform point)
        {
            if (!_inspectable)
            {
                return;
            }

            _inspectPoint = point;
            _isBeingInspected = true;
        }

        public void CancelInspect()
        {
            _isBeingInspected = false;
        }
    }
}