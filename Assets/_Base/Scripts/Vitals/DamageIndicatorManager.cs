using FireRingStudio.Pool;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.Vitals
{
    public class DamageIndicatorManager : MonoBehaviour
    {
        [SerializeField] private GameObject _damageIndicatorPrefab;
        
        private Health _playerHealth;

        private readonly List<DamageIndicator> _damageIndicators = new();


        private void Awake()
        {
            _playerHealth = GameObjectID.FindComponentInChildrenWithID<Health>(GameObjectID.PlayerID, true);
        }

        private void OnEnable()
        {
            if (_playerHealth != null)
            {
                _playerHealth.onTakeDamage.AddListener(IndicateDamage);
            }
        }

        private void OnDisable()
        {
            if (_playerHealth != null)
            {
                _playerHealth.onTakeDamage.RemoveListener(IndicateDamage);
            }
        }

        private void IndicateDamage(Damage damage)
        {
            Transform playerTransform = _playerHealth.transform;
            if (Vector3.Distance(playerTransform.position, damage.Position) < 0.1f)
            {
                return;
            }
            
            DamageIndicator damageIndicator = GetDamageIndicator(damage);
            
            damageIndicator.Initialize(playerTransform, damage);
        }

        private DamageIndicator CreateDamageIndicator()
        {
            if (_damageIndicatorPrefab == null)
            {
                return null;
            }

            PooledObject instance = _damageIndicatorPrefab.Get(transform);
            
            Transform instanceTransform = instance.transform;
            instanceTransform.localPosition = Vector3.zero;
            instanceTransform.localScale = Vector3.one;

            DamageIndicator damageIndicator = _damageIndicators.Find(x => x.gameObject == instance.gameObject);
            if (damageIndicator == null && instance.TryGetComponent(out damageIndicator))
            {
                _damageIndicators.Add(damageIndicator);
            }

            return damageIndicator;
        }

        private DamageIndicator GetDamageIndicator(Damage damage)
        {
            if (damage.Transform != null)
            {
                DamageIndicator damageIndicator = _damageIndicators.Find(x => x.Damage.Transform == damage.Transform);
                if (damageIndicator != null)
                {
                    return damageIndicator;
                }
            }
            
            return CreateDamageIndicator();
        }
    }
}