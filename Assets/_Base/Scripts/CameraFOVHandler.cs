using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(Camera))]
    public class CameraFOVHandler : MonoBehaviour
    {
        [SerializeField] private Camera _targetCamera;
        [HideIf("@_targetCamera != null")]
        [SerializeField] private float _target;
        [SerializeField, Min(0f)] private float _smoothTime = 0.3f;
        [SerializeField] private bool _restoreOnEnable;

        private Camera _camera;
        
        private float _defaultTarget;
        private float _velocity;

        public float Target
        {
            get => _target;
            set => _target = value;
        }


        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _defaultTarget = _camera.fieldOfView;
            ResetToDefault();
        }

        private void OnEnable()
        {
            if (_restoreOnEnable)
            {
                ResetToDefault();
            }
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            float target = _targetCamera != null ? _targetCamera.fieldOfView : _target;
            
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, target, ref _velocity, _smoothTime);
        }

        public void Snap()
        {
            float target = _targetCamera != null ? _targetCamera.fieldOfView : _target;
            _camera.fieldOfView = target;
        }

        public void ResetToDefault()
        {
            _target = _defaultTarget;
        }
    }
}