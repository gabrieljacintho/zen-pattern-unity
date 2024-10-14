using System;
using FireRingStudio.Physics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterGravity : MonoBehaviour
    {
        [SerializeField] private Detector _groundDetector;
        
        [Header("Settings")]
        [SerializeField] private float _gravityScaleOnAir = 1f;
        [SerializeField] private float _gravityScaleOnGround = 0.1f;
        [SerializeField] private bool _resetOnGameStart = true;

        [Space]
        [ReadOnly] public Vector3 Velocity = Vector3.zero;
        
        private CharacterController _characterController;

        public float GravityScaleOnAir => _gravityScaleOnAir;
        public float GravityScaleOnGround => _gravityScaleOnGround;
        private bool IsGrounded => _groundDetector != null && _groundDetector.IsDetecting;
        

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (_resetOnGameStart)
            {
                GameManager.GameStarted += ResetVelocity;
            }
            ResetVelocity();
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            float gravity = UnityEngine.Physics.gravity.y;
            if (IsGrounded)
            {
                if (Velocity.y <= 0f)
                {
                    Velocity.y = gravity * _gravityScaleOnGround;
                }
            }
            else
            {
                Velocity.y += gravity * _gravityScaleOnAir * Time.fixedDeltaTime;
            }

            _characterController.Move(Velocity * Time.fixedDeltaTime);
        }
        
        private void OnDisable()
        {
            if (_resetOnGameStart)
            {
                GameManager.GameStarted -= ResetVelocity;
            }
        }

        public void ResetVelocity()
        {
            Velocity = Vector3.zero;
        }
    }
}