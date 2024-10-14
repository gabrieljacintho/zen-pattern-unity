using System;
using FireRingStudio.Character;
using FireRingStudio.Physics;
using FireRingStudio.Surface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    [RequireComponent(typeof(Health))]
    public class FallDamage : MonoBehaviour
    {
        [SerializeField] private Caster _groundCaster;
        
        [Header("Settings")]
        [SerializeField] private float _heightThreshold = 3f;
        [SerializeField] private float _damagePerSpeed = 20f;
        [SerializeField] private float _velocityToSubtract = -9.81f;
        [SerializeField] private bool _playImpactEffect = true;
        [ShowIf("_playImpactEffect")]
        [SerializeField] private string _impactEffectId;
        
        private Health _health;
        private CharacterGravity _characterGravity;
        private Rigidbody _rigidbody;
        
        private float _initialFallHeight;
        private float _lastVelocity;

        private bool _wasFalling;

        public bool IsFalling => Velocity < 0f && !IsGrounded;

        private float Velocity
        {
            get
            {
                if (_characterGravity != null)
                {
                    return _characterGravity.Velocity.y;
                }
                
                if (_rigidbody != null)
                {
                    return _rigidbody.velocity.y;
                }

                return 0f;
            }
        }
        private bool IsGrounded => _groundCaster != null && _groundCaster.IsDetecting;
        
        [Space]
        public UnityEvent onFall;


        private void Awake()
        {
            _health = GetComponent<Health>();

            if (TryGetComponent(out CharacterGravity characterGravity))
            {
                _characterGravity = characterGravity;
            }
            else if (TryGetComponent(out Rigidbody rigidbody))
            {
                _rigidbody = rigidbody;
            }

            GameManager.GameStarted += Restore;
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused || Time.time - GameManager.GameStartTime < 1f)
            {
                return;
            }

            if (!_wasFalling && IsFalling)
            {
                _initialFallHeight = transform.position.y;
            }
            else if (_wasFalling && !IsFalling)
            {
                Fall(_lastVelocity);
            }
            
            if (IsFalling)
            {
                _lastVelocity = Velocity;
            }

            _wasFalling = IsFalling;
        }

        private void OnDestroy()
        {
            GameManager.GameStarted -= Restore;
        }

        public void Restore()
        {
            _wasFalling = false;
        }

        private void Fall(float velocity)
        {
            float fallHeight = _initialFallHeight - transform.position.y;
            if (fallHeight >= _heightThreshold)
            {
                ApplyFallDamage(velocity - _velocityToSubtract);
            }
            
            if (_playImpactEffect)
            {
                TryPlayImpactEffect();
            }
                    
            onFall?.Invoke();
        }

        private void ApplyFallDamage(float velocity)
        {
            if (_health == null || velocity >= 0f)
            {
                return;
            }

            float amount = Mathf.Max(_damagePerSpeed * -velocity, 0f);
            if (amount == 0f)
            {
                return;
            }
            
            Damage damage = new Damage(DamageType.Impact, amount);
            
            _health.TakeDamage(damage);
        }

        private bool TryPlayImpactEffect()
        {
            if (!IsGrounded)
            {
                return false;
            }
            
            SurfaceManager.SpawnEffect(_groundCaster.Hits[0], SurfaceEffectType.FallImpact, _impactEffectId, false);

            return true;
        }
    }
}