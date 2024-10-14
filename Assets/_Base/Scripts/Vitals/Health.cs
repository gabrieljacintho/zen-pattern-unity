using System;
using System.Collections;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Operations;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    [Serializable]
    public struct DamageResistance
    {
        public DamageType Type;
        [Range(-2f, 1f)] public float Resistance;
    }
    
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private FloatReference _healthReference = new(100f);
        [SerializeField] private FloatReference _maxHealthReference = new(100f);
        [SerializeField, ES3NonSerializable] private FloatMultiplication _maxHealthScale = new(1f);
        [SerializeField, ES3NonSerializable] private List<DamageResistance> _resistances;

        [Header("Settings")]
        [SerializeField, ES3NonSerializable] private float _extendedDamageHitDelay = 1f;
        [SerializeField, ES3NonSerializable] private bool _autoRevive;
        [ShowIf("_autoRevive")]
        [SerializeField, ES3NonSerializable] private float _reviveDelay = 30f;
        [SerializeField, ES3NonSerializable] private bool _releaseToPoolOnDie;

        [Space]
        [SerializeField, ES3NonSerializable] private bool _reviveOnGameStart;
        
        [Header("Audios")]
        [SerializeField, ES3NonSerializable] private EventReference _hurtSFX;
        [SerializeField, ES3NonSerializable] private EventReference _deathSFX;
        [Tooltip("If null the component Transform is used.")]
        [SerializeField, ES3NonSerializable] private Transform _audioParent;

        private readonly Dictionary<DamageType, Damage> _extendedDamagesByType = new();
        private readonly Dictionary<DamageType, Coroutine> _damageCoroutines = new();

        [Space]
        [ES3NonSerializable] public UnityEvent<Damage> onTakeDamage;
        [ES3NonSerializable] public UnityEvent<float> onTakeDamageAmount;
        [ES3NonSerializable] public UnityEvent onDie;
        [ES3NonSerializable] public UnityEvent onRevive;

        public bool IsDead { get; private set; }

        [ES3Serializable]
        public float CurrentHealth
        {
            get => _healthReference != null ? _healthReference.Value : 0f;
            set
            {
                if (_healthReference == null)
                {
                    _healthReference = new FloatReference(0f);
                }
                
                _healthReference.Value = Mathf.Clamp(value, 0f, MaxHealth);
            }
        }
        public float MaxHealth => _maxHealthReference != null ? _maxHealthReference.Value * _maxHealthScale.Result : 0f;


        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        private void OnEnable()
        {
            if (_reviveOnGameStart)
            {
                GameManager.GameStarted += Revive;
            }
        }
        
        private void OnDisable()
        {
            if (_reviveOnGameStart)
            {
                GameManager.GameStarted -= Revive;
            }
        }

        public void TakeDamage(Damage damage)
        {
            if (IsDead)
            {
                return;
            }

            float resistance = 0f;
            if (_resistances != null)
            {
                resistance = _resistances.Find(resistance => resistance.Type == damage.Type).Resistance;
            }
            
            float amount = damage.Amount;
            amount -= damage.Amount * resistance;

            float duration = damage.Duration;
            if (resistance > 0)
            {
                duration -= damage.Duration * resistance;
            }

            damage = new Damage(damage.Type, amount, duration, damage.Position, damage.Transform);

            if (_extendedDamageHitDelay > 0f && damage.Duration > 0f)
            {
                StartExtendedDamage(damage);
            }
            else
            {
                TakeDamage(damage.Amount);
            }
            
            onTakeDamage?.Invoke(damage);
            
            Debug.Log("Damage taken: " + damage, this);
        }

        [HideIf("IsDead")]
        [Button]
        public void Die()
        {
            if (IsDead)
            {
                return;
            }
            
            CurrentHealth = 0f;
            IsDead = true;
            
            StopAllCoroutines();
            
            onDie?.Invoke();

            if (!_deathSFX.IsNull)
            {
                _deathSFX.Play(transform.position);
            }

            if (_autoRevive)
            {
                if (_reviveDelay > 0f)
                {
                    this.DoAfterSeconds(Revive, _reviveDelay);
                }
                else
                {
                    Revive();
                }
            }
            else if (_releaseToPoolOnDie)
            {
                gameObject.ReleaseToPool();
            }
        }
        
        [ShowIf("@CurrentHealth < MaxHealth")]
        [Button]
        public void Revive()
        {
            if (!IsDead && CurrentHealth >= MaxHealth)
            {
                return;
            }

            CurrentHealth = MaxHealth;
            ClearExtendedDamages();

            IsDead = false;
            
            onRevive?.Invoke();
        }
        
        public bool TryGetExtendedDamage(DamageType type, out Damage damage)
        {
            return _extendedDamagesByType.TryGetValue(type, out damage);
        }

        public void ClearExtendedDamages()
        {
            _extendedDamagesByType.Clear();
            StopAllCoroutines();
        }

        private void TakeDamage(float damageAmount)
        {
            if (IsDead)
            {
                return;
            }
            
            CurrentHealth -= damageAmount;
            
            if (CurrentHealth <= 0f)
            {
                Die();
            }
            else if (!_hurtSFX.IsNull)
            {
                if (_audioParent != null)
                {
                    _hurtSFX.Play(_audioParent, EmitterGameEvent.ObjectDisable, false);
                }
                else
                {
                    _hurtSFX.Play(transform, EmitterGameEvent.ObjectDisable, false);
                }
            }
            
            onTakeDamageAmount?.Invoke(damageAmount);
        }

        private void StartExtendedDamage(Damage damage)
        {
            if (TryGetExtendedDamage(damage.Type, out Damage currentDamage))
            {
                damage.Amount = Mathf.Max(currentDamage.Amount, damage.Amount);
                damage.Duration = Mathf.Max(currentDamage.Duration, damage.Duration);
                _extendedDamagesByType[damage.Type] = damage;
            }
            else
            {
                _extendedDamagesByType.Add(damage.Type, damage);
            }

            Coroutine coroutine = StartCoroutine(TakeExtendedDamage(damage));
            if (_damageCoroutines.TryGetValue(damage.Type, out Coroutine currentCoroutine))
            {
                StopCoroutine(currentCoroutine);
                _damageCoroutines[damage.Type] = coroutine;
            }
            else
            {
                _damageCoroutines.Add(damage.Type, coroutine);
            }
        }
        
        private IEnumerator TakeExtendedDamage(Damage damage)
        {
            float hitDelay = Mathf.Min(damage.Duration, _extendedDamageHitDelay);
            int amountOfHits = Mathf.RoundToInt(damage.Duration / hitDelay);
            hitDelay = damage.Duration / amountOfHits;
            
            float hitDamageAmount = damage.Amount * (hitDelay / damage.Duration);
            
            float nextHitTime = damage.Duration;
            while (damage.Duration > 0f)
            {
                if (damage.Duration <= nextHitTime)
                {
                    TakeDamage(hitDamageAmount);
                    nextHitTime -= hitDelay;
                }
                
                damage.Duration -= Time.deltaTime;
                _extendedDamagesByType[damage.Type] = damage;

                yield return null;
            }

            _damageCoroutines[damage.Type] = null;
        }
    }
}