using FireRingStudio.Character;
using FireRingStudio.Extensions;
using FireRingStudio.Physics;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Movement
{
    [RequireComponent(typeof(CharacterGravity))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private Detector _groundDetector;
        
        [Header("Inputs")]
        [SerializeField] private InputActionReference _jumpInput;
        
        [Header("Settings")]
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _crouchJumpForce = 3f;
        [SerializeField] private float _minWaitTime = 1f;

        [Header("Audios")]
        [SerializeField] private EventReference _jumpSFX;

        public CharacterGravity CharacterGravity { get; private set; }
        public PlayerCrouch PlayerCrouch { get; private set; }

        [Space]
        public UnityEvent OnJump;

        private float _lastJumpTime;


        private void Awake()
        {
            CharacterGravity = GetComponent<CharacterGravity>();
            PlayerCrouch = GetComponentInChildren<PlayerCrouch>();
        }

        private void OnEnable()
        {
            if (_jumpInput != null)
            {
                _jumpInput.action.Reset();
                _jumpInput.action.performed += Jump;
            }
        }

        private void OnDisable()
        {
            if (_jumpInput != null)
            {
                _jumpInput.action.performed -= Jump;
            }
        }

        public void Jump()
        {
            if (CharacterGravity == null)
            {
                return;
            }
            
            if (GameManager.IsPaused || Time.time < _lastJumpTime + _minWaitTime)
            {
                return;
            }

            if (_groundDetector != null && !_groundDetector.IsDetecting)
            {
                return;
            }

            float jumpForce = PlayerCrouch != null && PlayerCrouch.IsCrouched ? _crouchJumpForce : _jumpForce;

            CharacterGravity.Velocity = Vector3.up * jumpForce;
            
            _lastJumpTime = Time.time;

            if (!_jumpSFX.IsNull)
            {
                _jumpSFX.Play(transform.position);
            }
            
            OnJump?.Invoke();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (GameManager.InGame)
            {
                Jump();
            }
        }
    }
}
