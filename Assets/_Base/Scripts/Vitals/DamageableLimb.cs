using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    [RequireComponent(typeof(Collider))]
    public class DamageableLimb : MonoBehaviour, IDamageable
    {
        [SerializeField, Range(-5f, 1f)] private float _resistance;

        [Space]
        public UnityEvent<Damage> onTakeDamage;
        
        public Health Health { get; private set; }
        public float CurrentHealth => Health != null ? Health.CurrentHealth : 0f;
        

        private void Awake()
        {
            Health = GetComponentInParent<Health>();
            if (Health == null)
                Debug.LogNoInParent(nameof(Vitals.Health));
        }

        public void TakeDamage(Damage damage)
        {
            damage.Amount *= 1 - _resistance;

            if (Health != null)
            {
                Health.TakeDamage(damage);
            }
            
            onTakeDamage?.Invoke(damage);
        }
    }
}
