using FireRingStudio.Extensions;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCrouch : MonoBehaviour
    {
        [SerializeField] private Transform _cameraRoot;
        
        [Header("Inputs")]
        [SerializeField] private InputActionReference _crouchInput;
        [SerializeField] private InputActionReference _standUpInput;
        [SerializeField] private InputActionReference _toggleInput;
        
        [Header("Settings")]
        [SerializeField] private float _standUpCharacterHeight = 2f;
        [SerializeField] private float _crouchCharacterHeight = 1.4f;
        [SerializeField] private float _cameraHeightOffset = -0.2f;
        [SerializeField] private float _crouchMoveSpeedScale = 0.25f;
        [SerializeField] private float _smoothTime = 0.2f;
        [SerializeField] private bool _crouchOnEnable;

        [Header("Audios")]
        [SerializeField] private EventReference _crouchSFX;
        [SerializeField] private EventReference _standUpSFX;
        
        private CharacterController _characterController;
        private PlayerMovement _playerMovement;

        private bool _isCrouched;

        public bool IsCrouched
        {
            get => _isCrouched;
            set
            {
                if (value)
                {
                    Crouch();
                }
                else
                {
                    StandUp();
                }
            }
        }
        
        [Space]
        public UnityEvent OnCrouch;
        public UnityEvent OnStandUp;

        private float _characterTargetHeight;
        private float _characterVelocity;
        
        private float _cameraTargetHeight;
        private float _cameraVelocity;


        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (_crouchInput != null)
            {
                _crouchInput.action.Reset();
                _crouchInput.action.performed += Crouch;
            }
            
            if (_standUpInput != null)
            {
                _standUpInput.action.Reset();
                _standUpInput.action.performed += StandUp;
            }
            
            if (_toggleInput != null)
            {
                _toggleInput.action.Reset();
                _toggleInput.action.performed += Toggle;
            }

            if (_crouchOnEnable)
            {
                Crouch();
            }
            else
            {
                StandUp();
            }
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }
            
            UpdateCharacter();
            UpdateCamera();
            UpdatePlayerMovement();
        }
        
        private void OnDisable()
        {
            if (_crouchInput != null)
            {
                _crouchInput.action.performed -= Crouch;
            }

            if (_standUpInput != null)
            {
                _standUpInput.action.performed -= StandUp;
            }

            if (_toggleInput != null)
            {
                _toggleInput.action.performed -= Toggle;
            }
        }
        
        public void Crouch()
        {
            _characterTargetHeight = _crouchCharacterHeight;
            _cameraTargetHeight = _crouchCharacterHeight + _cameraHeightOffset;

            if (_isCrouched)
            {
                return;
            }
            
            _isCrouched = true;

            if (!_crouchSFX.IsNull)
            {
                _crouchSFX.Play(transform.position);
            }

            OnCrouch?.Invoke();
        }

        private void Crouch(InputAction.CallbackContext context)
        {
            if (GameManager.InGame)
            {
                Crouch();
            }
        }
        
        public void StandUp()
        {
            _characterTargetHeight = _standUpCharacterHeight;
            _cameraTargetHeight = _standUpCharacterHeight + _cameraHeightOffset;
            
            if (!_isCrouched)
            {
                return;
            }

            _isCrouched = false;

            if (!_standUpSFX.IsNull)
            {
                _standUpSFX.Play(transform.position);
            }

            OnStandUp?.Invoke();
        }
        
        private void StandUp(InputAction.CallbackContext context)
        {
            if (GameManager.InGame)
            {
                StandUp();
            }
        }
        
        public void Toggle()
        {
            if (IsCrouched)
            {
                StandUp();
            }
            else
            {
                Crouch();
            }
        }

        private void Toggle(InputAction.CallbackContext context)
        {
            if (GameManager.InGame)
            {
                Toggle();
            }
        }

        private void UpdateCharacter()
        {
            if (_characterController == null)
            {
                return;
            }

            float height = _characterController.height;
            height = Mathf.SmoothDamp(height, _characterTargetHeight, ref _characterVelocity, _smoothTime);
            
            _characterController.height = height;
            _characterController.center = Vector3.up * _characterController.height / 2f;
        }
        
        private void UpdateCamera()
        {
            if (_cameraRoot == null)
            {
                return;
            }

            float height = _cameraRoot.localPosition.y;
            height = Mathf.SmoothDamp(height, _cameraTargetHeight, ref _cameraVelocity, _smoothTime);

            _cameraRoot.localPosition = Vector3.up * height;
        }

        private void UpdatePlayerMovement()
        {
            if (_playerMovement == null)
            {
                return;
            }

            float speedScale = _isCrouched ? _crouchMoveSpeedScale : 1f;
            _playerMovement.SpeedScale.SetComponentValue(this, speedScale);
        }
    }
}