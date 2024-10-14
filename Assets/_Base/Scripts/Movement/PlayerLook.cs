using FireRingStudio.Input;
using FireRingStudio.Operations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio.Movement
{
    public class PlayerLook : MonoBehaviour
    {
        public static float SensitivityScale = 0.55f;

        [Tooltip("If null the component Transform is used.")]
        [SerializeField, ES3NonSerializable] private Transform _root;
        
        [Header("Inputs")]
        [SerializeField, ES3NonSerializable] private InputActionReference _lookInput;
        [SerializeField, ES3NonSerializable] private InputActionReference _allowLookInput;
        
        [Header("Settings")]
        [SerializeField, ES3NonSerializable] private Vector2 _sensitivity = new Vector2(1, 1);
        [SerializeField, ES3NonSerializable] private Vector2 _range = new Vector2(360, 160);
        [SerializeField, ES3NonSerializable] private bool _restoreOnEnable;
        [SerializeField, ES3NonSerializable] private bool _onlyInGame;
        
        [Header("Gamepad")]
        [SerializeField, Range(0f, 1f), ES3NonSerializable] private float _initialVelocityScale;
        [SerializeField, ES3NonSerializable] private float _acceleration = 1f;

        [Space]
        [ReadOnly, ES3NonSerializable] public FloatMultiplication SensitivityScaleModifier = new(1f);
        
        [ES3Serializable] private Vector2 _targetRotation;
        [ES3Serializable] private Vector2 _initialLook;

        private float _velocityScale;

        public Vector2 Delta { get; private set; }
        public Transform Root => _root != null ? _root : transform;


        private void OnEnable()
        {
            if (_restoreOnEnable)
            {
                Restore();
            }
        }

        private void Update()
        {
            if (_onlyInGame && !GameManager.InGame)
            {
                return;
            }

            if (_lookInput == null)
            {
                return;
            }

            if (_allowLookInput != null && !_allowLookInput.action.IsPressed())
            {
                return;
            }
            
            Vector2 look = _lookInput.action.ReadValue<Vector2>();
            UpdateVelocityScale(look);

            Delta = look * _sensitivity * (SensitivityScaleModifier.Result * SensitivityScale * _velocityScale);
            
            _targetRotation += Delta;
            FixTargetRotation();

            look = _targetRotation;
            look.x += _initialLook.y;
            look.y += _initialLook.x;

            if (Root != transform)
            {
                transform.localRotation = Quaternion.AngleAxis(-look.y, Vector3.right);
                Root.localRotation = Quaternion.AngleAxis(look.x, Vector3.up);
            }
            else
            {
                Root.localRotation = Quaternion.Euler(-look.y, look.x, 0f);
            }
        }

        public void Restore()
        {
            if (Root != transform)
            {
                transform.localRotation = Quaternion.identity;
            }
            
            _targetRotation = Vector2.zero;
            _initialLook = Root.eulerAngles;
            _initialLook = new Vector2(-_initialLook.x, _initialLook.y);
            _velocityScale = _initialVelocityScale;
        }

        private void FixTargetRotation()
        {
            if (_range.x < 360f)
            {
                _targetRotation.x = Mathf.Clamp(_targetRotation.x, -_range.x / 2, _range.x / 2);
            }

            if (_range.y < 360f)
            {
                _targetRotation.y = Mathf.Clamp(_targetRotation.y, -_range.y / 2, _range.y / 2);
            }

            if (_targetRotation.x >= 360f)
            {
                _targetRotation.x -= 360f;
            }
            else if (_targetRotation.x <= -360f)
            {
                _targetRotation.x += 360f;
            }
            
            if (_targetRotation.y >= 360f)
            {
                _targetRotation.y -= 360f;
            }
            else if (_targetRotation.y <= -360f)
            {
                _targetRotation.y += 360f;
            }
        }
        
        private void UpdateVelocityScale(Vector2 look)
        {
            if (InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse)
            {
                _velocityScale = 1f;
            }
            else if (look.magnitude > 0.01f)
            {
                _velocityScale += _acceleration * Time.deltaTime;
            }
            else
            {
                _velocityScale = _initialVelocityScale;
            }
            
            _velocityScale = Mathf.Clamp(_velocityScale, 0f, 1f);
        }
    }
}