using FireRingStudio.Input;
using FireRingStudio.Operations;
using FireRingStudio.Physics;
using FireRingStudio.Vitals;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Movement
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Airborne
    }

    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Detector _groundDetector;
        [SerializeField] private Caster _groundCaster;
        [SerializeField] private Caster _wallCaster;

        [Header("Inputs")]
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private InputActionReference _sprintInput;

        [Header("Settings")]
        [SerializeField] private FloatReference _walkSpeedReference = new (3f);
        [SerializeField] private FloatReference _sprintSpeedReference = new (6f);
        [SerializeField] private FloatReference _airborneSpeedReference = new (1.5f);
        [SerializeField] private BoolAndOperation _canSprint = new (true);

        [Header("Acceleration/Deceleration")]
        [SerializeField] private float _accelerationSpeed = 10f;
        [SerializeField] private float _decelerationSpeed = 20f;

        [Header("Speed Scales")]
        [SerializeField] private FloatMultiplication _speedScale = new (1f);
        [SerializeField] private float _backwardSpeedScale = 0.6f;
        [SerializeField] private float _lateralSpeedScale = 0.6f;
        
        private CharacterController _characterController;
        private Stamina _stamina;

        private MovementState _currentState;
        private Vector3 _lastMove;

        private float _currentSpeed;
        private float _targetSpeed;

        public BoolAndOperation CanSprint => _canSprint;
        public FloatMultiplication SpeedScale => _speedScale;
        public MovementState CurrentState => _currentState;
        public float CurrentSpeed => _currentSpeed;

        [Space]
        public UnityEvent<MovementState> OnStateChanged;


        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _stamina = GetComponent<Stamina>();
        }

        private void Update()
        {
            if (!GameManager.InGame || _moveInput == null || _characterController == null)
            {
                return;
            }

            Vector3 move = _moveInput.action.ReadValue<Vector2>();
            if (move.x != 0)
            {
                move.x *= _lateralSpeedScale;
            }

            if (move.y < 0)
            {
                move.y *= _backwardSpeedScale;
            }

            move = transform.right * move.x + transform.forward * move.y;

            if (_groundCaster != null && _groundCaster.Hits.Count > 0)
            {
                Vector3 normal = _groundCaster.Hits[0].normal;
                move = Vector3.ProjectOnPlane(move, normal);
            }

            if (_wallCaster != null && move.magnitude > 0f)
            {
                _wallCaster.transform.LookAt(_wallCaster.transform.position + move);
            }

            UpdateState(move);

            if (move.magnitude > 0f || CurrentState == MovementState.Airborne)
            {
                _lastMove = move;
            }

            float speed = _targetSpeed > CurrentSpeed ? _accelerationSpeed : _decelerationSpeed;
            _currentSpeed = Mathf.MoveTowards(CurrentSpeed, _targetSpeed, speed * Time.deltaTime);

            _characterController.enabled = true;
            _characterController.Move(CurrentSpeed * _speedScale.Result * Time.deltaTime * _lastMove);
        }
        
        public void StopMoving()
        {
            if (_moveInput != null)
            {
                _moveInput.action.Reset();
            }
        }
        
        public void StopSprinting()
        {
            if (_sprintInput != null)
            {
                _sprintInput.action.Reset();
            }
        }

        private void UpdateState(Vector3 move)
        {
            if (_groundDetector != null && !_groundDetector.IsDetecting)
            {
                ChangeState(MovementState.Airborne);
            }
            else if (_wallCaster != null && _wallCaster.IsDetecting)
            {
                ChangeState(MovementState.Idle);
            }
            else if (move.magnitude <= 0)
            {
                ChangeState(MovementState.Idle);
            }
            else if (_canSprint.Result && _sprintInput != null && (_stamina == null || !_stamina.IsTired))
            {
                if (InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse)
                {
                    ChangeState(_sprintInput.action.IsPressed() ? MovementState.Sprinting : MovementState.Walking);
                }
                else if (CurrentState == MovementState.Sprinting || _sprintInput.action.WasPressedThisFrame())
                {
                    ChangeState(MovementState.Sprinting);
                }
                else
                {
                    ChangeState(MovementState.Walking);
                }
            }
            else
            {
                ChangeState(MovementState.Walking);
            }
        }

        private void ChangeState(MovementState state)
        {
            if (_currentState == state)
            {
                return;
            }

            switch (state)
            {
                case MovementState.Idle:
                    _targetSpeed = 0;
                    break;

                case MovementState.Walking:
                    _targetSpeed = _walkSpeedReference?.Value ?? 0f;
                    break;

                case MovementState.Sprinting:
                    _targetSpeed = _sprintSpeedReference?.Value ?? 0f;
                    break;

                case MovementState.Airborne:
                    float airborneSpeed = _airborneSpeedReference?.Value ?? 0f;
                    _targetSpeed = Mathf.Max(CurrentSpeed, airborneSpeed);
                    break;
            }

            _currentState = state;
            
            OnStateChanged?.Invoke(_currentState);
        }
    }
}