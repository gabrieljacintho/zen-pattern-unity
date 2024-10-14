using FireRingStudio.FPS;
using FireRingStudio.Operations;
using FireRingStudio.Physics;
using UnityEngine;

namespace FireRingStudio.Vitals
{
    public class DamageArea : HitArea
    {
        [Header("Damage")]
        [SerializeField] private DamageParameters _damageParameters;
        [SerializeField] private FloatMultiplication _damageScale = new(1f);

        public DamageParameters DamageParameters
        {
            get => _damageParameters;
            set => _damageParameters = value;
        }
        protected override LayerMask LayerMask => _damageParameters.LayerMask;
        protected override QueryTriggerInteraction QueryTriggerInteraction => _damageParameters.TriggerInteraction;
        

        protected override void OnHit(Collider other, RaycastHit hit)
        {
            if (hit.collider != other || !other.TryGetComponent(out IDamageable damageable))
            {
                return;
            }

            damageable.TakeDamage(GetDamage());
        }

        private Damage GetDamage()
        {
            float damageAmount = UnityEngine.Random.Range(_damageParameters.MinAmount, _damageParameters.MaxAmount);
            damageAmount *= _damageScale.Result;
            
            if (_damageParameters.CriticalChance > 0f && UnityEngine.Random.Range(0f, 1f) <= _damageParameters.CriticalChance)
            {
                damageAmount *= _damageParameters.CriticalScale;
            }
            
            return new Damage(_damageParameters.Type, damageAmount, _damageParameters.Duration, transform);
        }
    }
}