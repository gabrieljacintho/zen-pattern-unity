using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using FireRingStudio.Operations;
using FireRingStudio.Vitals;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.FPS
{
    public class Explosive : MonoBehaviour
    {
        [SerializeField] private float _force = 6f;
        [SerializeField] private float _radius = 2f;
        [SerializeField] private float _upwardsModifier = 1f;
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] protected LayerMask _obstacleLayerMask;
        [SerializeField] protected QueryTriggerInteraction _triggerInteraction;
        [SerializeField] private bool _detonateOnEnable;
        
        [Header("Damage")]
        [SerializeField] private FloatReference _minDamageAmountReference = new(80f);
        [SerializeField] private FloatReference _maxDamageAmountReference = new(90f);
        [SerializeField] private FloatMultiplication _damageAmountScale = new(1f);

        [Header("FXs")]
        [SerializeField] private EventReference _explosionSFX;
        [SerializeField] private GameObject _vfxPrefab;

        private float MinDamageAmount => _minDamageAmountReference?.Value ?? 0f;
        private float MaxDamageAmount => _maxDamageAmountReference?.Value ?? 0f;
        
        [Space]
        [FormerlySerializedAs("onDetonate")]
        public UnityEvent OnDetonate;


        private void OnEnable()
        {
            if (_detonateOnEnable)
            {
                Detonate();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        [Button]
        public void Detonate()
        {
            if (_vfxPrefab != null)
            {
                _vfxPrefab.Get(transform.position);
            }
            
            Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, _radius, _targetLayerMask);
            int amount = colliders != null ? colliders.Length : 0;
            
            for (int i = 0; i < amount; i++)
            {
                Transform target = colliders[i].transform;
                if (target == transform || PhysicsHelper.HasObstacleBetween(transform.position, colliders[i], _obstacleLayerMask, _triggerInteraction))
                {
                    continue;
                }
                
                ApplyForce(target);
                ApplyDamage(target);
            }

            if (!_explosionSFX.IsNull)
            {
                _explosionSFX.Play(transform.position);
            }
            
            OnDetonate?.Invoke();
        }

        private void ApplyForce(Transform target)
        {
            if (_force != 0f && target.TryGetComponent(out Rigidbody body))
            {
                body.AddExplosionForce(_force, transform.position, _radius, _upwardsModifier, ForceMode.Impulse);
            }
        }

        private void ApplyDamage(Transform target)
        {
            if (MaxDamageAmount == 0f || !target.TryGetComponent(out IDamageable damageable))
            {
                return;
            }
            
            damageable.TakeDamage(GetDamage(target));
        }

        private Damage GetDamage(Transform target)
        {
            float damageAmount = UnityEngine.Random.Range(MinDamageAmount, MaxDamageAmount);
            damageAmount *= _damageAmountScale.Result;
            
            float distance = Vector3.Distance(target.position, transform.position);
            damageAmount *= Mathf.Abs(distance / _radius - 1);
                    
            return new Damage(DamageType.Explosion, damageAmount, transform.position);
        }
    }
}