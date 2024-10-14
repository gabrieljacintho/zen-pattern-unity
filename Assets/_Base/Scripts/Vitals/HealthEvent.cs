using System.Collections.Generic;
using FireRingStudio.FPS;
using FireRingStudio.FPS.HealingItem;
using FireRingStudio.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    public class HealthEvent : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [HideIf("@_health != null")]
        [SerializeField] private string _healthId;
        [SerializeField] private float _lowHealthThreshold = 30f;
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private int _updateInterval = 1;

        private bool _lowHealthWarningEnabled;
        
        private float Health
        {
            get
            {
                if (_health == null)
                {
                    _health = ComponentID.FindComponentWithID<Health>(_healthId);
                }

                return _health != null ? _health.CurrentHealth : 100f;
            }
        }
        
        [Space]
        public UnityEvent OnLowHealth;
        public UnityEvent OnNormalHealth;
        [SerializeField] private bool _alwaysInvoke;
        
        
        private void LateUpdate()
        {
            if (_updateInterval <= 0 || Time.frameCount % _updateInterval != 0)
            {
                return;
            }

            if (Health > _lowHealthThreshold)
            {
                SetLowHealthWarningEnabled(false);
                return;
            }

            if (_inventory != null)
            {
                if (!HasHealingItem(_inventory))
                {
                    SetLowHealthWarningEnabled(false);
                    return;
                }
            }

            SetLowHealthWarningEnabled(true);
        }

        private void SetLowHealthWarningEnabled(bool value)
        {
            if (_lowHealthWarningEnabled == value && !_alwaysInvoke)
            {
                return;
            }
            
            _lowHealthWarningEnabled = value;
            
            if (_lowHealthWarningEnabled)
            {
                OnLowHealth?.Invoke();
            }
            else
            {
                OnNormalHealth?.Invoke();
            }
        }

        private bool HasHealingItem(InventoryData inventory)
        {
            List<ItemPack> healingItemPacks = inventory.GetItemPacksOfDataType<HealingItemData>();
            if (healingItemPacks.Count > 0 && healingItemPacks.Exists(pack => !pack.IsEmpty))
            {
                return true;
            }

            return false;
        }
    }
}