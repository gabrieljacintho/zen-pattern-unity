using FireRingStudio.Helpers;
using FireRingStudio.Input;
using FireRingStudio.Movement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [DisallowMultipleComponent]
    public class ProjectileWeaponAim : ProjectileWeaponComponent
    {
        [SerializeField] private InputActionReference _aimInput;

        private PlayerMovement _playerMovement;
        
        private float _defaultPrecisionRadius;
        private float _currentPrecisionRadius;
        private float _targetPrecisionRadius;
        private bool _precisionInitialized;

        private bool _loadingScope;
        private float _scopeTime;
        
        public bool IsAiming { get; private set; }
        public Precision Precision { get; private set; }
        public bool CanShowScope { get; private set; }

        [Space]
        public UnityEvent OnStartAim;
        public UnityEvent OnStopAim;


        #region MonoBehaviour
        
        protected override void Awake()
        {
            base.Awake();
            _playerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnStateChanged.AddListener(OnPlayerMovementStateChanged);
            }
            
            ResetPrecision();
        }

        private void Update()
        {
            if (GameManager.IsPaused || !IsEquipped)
            {
                return;
            }

            UpdateInput();
            RemoveRecoil();

            if (Data != null)
            {
                MathHelper.DelayedAction(() =>
                {
                    CanShowScope = true;
                }, Data.ScopeDelay, ref _scopeTime, ref _loadingScope);
            }
        }
        
        private void OnDisable()
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnStateChanged.RemoveListener(OnPlayerMovementStateChanged);
            }
            
            StopAim();
        }
        
        #endregion
        
        public void StartAim()
        {
            if (IsAiming)
            {
                return;
            }
            
            if (Reload != null && Reload.IsReloading)
            {
                return;
            }

            if (Data != null && !Data.AimingState.AllowedWhileAirborne)
            {
                if (_playerMovement != null && _playerMovement.CurrentState == MovementState.Airborne)
                {
                    return;
                }
            }

            IsAiming = true;
            
            UpdatePrecision();
            
            OnStartAim?.Invoke();
        }

        public void StopAim()
        {
            if (!IsAiming)
            {
                return;
            }
            
            IsAiming = false;
            
            UpdatePrecision();
            
            OnStopAim?.Invoke();
        }
        
        public void ToggleAim()
        {
            if (!IsAiming)
            {
                StartAim();
            }
            else
            {
                StopAim();
            }
        }

        private void UpdateInput()
        {
            if (!GameManager.InGame || _aimInput == null)
            {
                return;
            }
            
            if (InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse)
            {
                if (_aimInput.action.WasPressedThisFrame())
                {
                    ToggleAim();
                }
            }
            else if (_aimInput.action.IsPressed())
            {
                StartAim();
            }
            else
            {
                StopAim();
            }
        }
        
        private void UpdatePrecision()
        {
            if (Data == null)
            {
                return;
            }
            
            MovementState movementState = _playerMovement != null ? _playerMovement.CurrentState : MovementState.Idle;
            
            if (IsAiming && movementState == MovementState.Airborne)
            {
                if (!Data.AimingState.AllowedWhileAirborne)
                {
                    StopAim();
                }
            }

            if (IsAiming)
            {
                Precision = Data.AimingPrecisions.GetValue(movementState);
            }
            else
            {
                Precision = Data.DefaultPrecisions.GetValue(movementState);
            }

            _defaultPrecisionRadius = Precision.Radius;
            
            if (!_precisionInitialized)
            {
                InitializePrecision();
            }
            else
            {
                Precision precision = Precision;
                precision.Radius = _currentPrecisionRadius;

                _loadingScope = Data.ScopeData != null && Data.ScopeDelay > 0f;
                _scopeTime = 0f;

                CanShowScope = Data.ScopeData != null && Data.ScopeDelay <= 0f;
                Precision = precision;
            }
        }

        private void InitializePrecision()
        {
            _currentPrecisionRadius = _defaultPrecisionRadius;
            _targetPrecisionRadius = _defaultPrecisionRadius;
            _precisionInitialized = true;
        }

        public void AddRecoil(float radius)
        {
            _targetPrecisionRadius += radius;
        }
        
        private void RemoveRecoil()
        {
            if (Data == null)
            {
                return;
            }

            float returnSpeed = Data.PrecisionReturnSpeed;
            float snappiness = Data.PrecisionSnappiness;
            
            _targetPrecisionRadius = Mathf.Lerp(_targetPrecisionRadius, _defaultPrecisionRadius, returnSpeed * Time.deltaTime);
            _currentPrecisionRadius = Mathf.Lerp(_currentPrecisionRadius, _targetPrecisionRadius, snappiness * Time.deltaTime);

            Precision precision = Precision;
            precision.Radius = _currentPrecisionRadius;
            Precision = precision;
        }

        private void OnPlayerMovementStateChanged(MovementState movementState)
        {
            _precisionInitialized = false;
            UpdatePrecision();
        }

        private void ResetPrecision()
        {
            _precisionInitialized = false;
            UpdatePrecision();
        }
    }
}