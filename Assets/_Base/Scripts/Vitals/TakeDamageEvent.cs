using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    public class TakeDamageEvent : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private DamageType[] _requiredDamageTypes;

        [Space]
        public UnityEvent<Damage> OnTakeDamage;
        public UnityEvent<float> OnTakeDamageAmount;
        public UnityEvent OnDie;


        private void OnEnable()
        {
            if (_health != null)
            {
                _health.onTakeDamage.AddListener(OnTakeDamageFunc);
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.onTakeDamage.RemoveListener(OnTakeDamageFunc);
            }
        }

        private void OnTakeDamageFunc(Damage damage)
        {
            if (_requiredDamageTypes != null && _requiredDamageTypes.Length > 0 && !_requiredDamageTypes.Contains(damage.Type))
            {
                return;
            }
            
            OnTakeDamage?.Invoke(damage);
            OnTakeDamageAmount?.Invoke(damage.Amount);

            if (_health.IsDead)
            {
                OnDie?.Invoke();
            }
        }
    }
}