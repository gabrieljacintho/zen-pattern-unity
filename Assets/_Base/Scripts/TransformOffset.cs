using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    public class TransformOffset : MonoBehaviour
    {
        [FormerlySerializedAs("speed")]
        [SerializeField] private float _speed = 1f;
        [FormerlySerializedAs("restoreOnEnable")]
        [SerializeField] private bool _restoreOnEnable;

        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        
        private Vector3 _positionOffset;
        private Vector3 _rotationOffset;

        private float _t;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        public Vector3 PositionOffset
        {
            get => _positionOffset;
            set
            {
                if (_positionOffset != value)
                {
                    _positionOffset = value;
                    _t = 0f;
                }
            }
        }
        public Vector3 RotationOffset
        {
            get => _rotationOffset;
            set
            {
                if (_rotationOffset != value)
                {
                    _rotationOffset = value;
                    _t = 0f;
                }
            }
        }


        private void Awake()
        {
            UpdateDefaultValues();
        }

        private void OnEnable()
        {
            if (_restoreOnEnable)
            {
                Restore();
            }
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }
            
            if (_t < 1f)
            {
                _t += _speed * Time.deltaTime;
            }

            UpdateOffset();
        }

        public void ApplyOffset()
        {
            _t = 1f;
            UpdateOffset();
        }

        public void UpdateDefaultValues()
        {
            _defaultPosition = transform.localPosition;
            _defaultRotation = transform.localRotation;
        }

        [Button]
        public void Restore()
        {
            PositionOffset = Vector3.zero;
            RotationOffset = Vector3.zero;

            transform.SetLocalPositionAndRotation(_defaultPosition, _defaultRotation);
        }

        private void UpdateOffset()
        {
            Vector3 targetPosition = _defaultPosition + PositionOffset;
            Quaternion targetRotation = _defaultRotation * Quaternion.Euler(RotationOffset);

            Vector3 localPosition = Vector3.Lerp(transform.localPosition, targetPosition, _t);
            Quaternion localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _t);
            
            transform.SetLocalPositionAndRotation(localPosition, localRotation);
        }
    }
}
