using FireRingStudio.Extensions;
using FireRingStudio.FPS;
using FireRingStudio.FPS.Equipment;
using FireRingStudio.FPS.HealingItem;
using FireRingStudio.FPS.MeleeWeapon;
using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.FPS.ThrowingWeapon;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Inventory
{
    public class CollectedEquipmentCount : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private IntVariable _intVariable;
        [SerializeField] private EquipmentType[] _requiredTypes;
        [SerializeField] private bool _useSave;
        [ShowIf("_useSave")]
        [SerializeField] private string _saveKey = "CollectedEquipmentCount";
        
        private int _count;
        
        [Space]
        public UnityEvent<int> OnChange;
        public UnityEvent OnContains;


        private void OnEnable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateCount;
            }
            
            UpdateCount();
        }
        
        private void OnDisable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateCount;
            }
        }

        public void UpdateCount()
        {
            if (_inventory == null)
            {
                SetCount(0);
                return;
            }

            int count = 0;
            foreach (ItemPack itemPack in _inventory.ItemPacks)
            {
                if (!CanCount(itemPack))
                {
                    continue;
                }
                    
                count++;

                if (_useSave)
                {
                    Save(itemPack);
                }
            }

            SetCount(count);
        }

        private void Save(ItemPack itemPack)
        {
            string saveKey = GetSaveKey(itemPack);
            if (saveKey == _saveKey)
            {
                return;
            }
            
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
        }
        
        private bool CanCount(ItemPack itemPack)
        {
            if (_useSave)
            {
                string saveKey = GetSaveKey(itemPack);
                if (saveKey != _saveKey && PlayerPrefs.GetInt(saveKey) == 1)
                {
                    return CanCount(itemPack.Data);
                }
            }

            return !itemPack.IsEmpty && CanCount(itemPack.Data);
        }

        private bool CanCount(ItemData itemData)
        {
            if (_requiredTypes == null || _requiredTypes.Length == 0)
            {
                return true;
            }
            
            switch (itemData)
            {
                case ProjectileWeaponData:
                    return _requiredTypes.Contains(EquipmentType.ProjectileWeapon);
                
                case MeleeWeaponData:
                    return _requiredTypes.Contains(EquipmentType.MeleeWeapon);
                
                case ThrowingWeaponData:
                    return _requiredTypes.Contains(EquipmentType.ThrowingWeapon);
                
                case HealingItemData:
                    return _requiredTypes.Contains(EquipmentType.HealingItem);
                
                case EquipmentData:
                    return _requiredTypes.Contains(EquipmentType.Tool);
            }

            return false;
        }
        
        private string GetSaveKey(ItemPack itemPack)
        {
            return _saveKey + (itemPack.Data != null ? "_" + itemPack.Data.Id : null);
        }
        
        private void SetCount(int value)
        {
            if (_intVariable != null)
            {
                _intVariable.Value = value;
            }

            if (_count == value)
            {
                return;
            }

            int lastCount = _count;
            _count = value;
            
            OnChange?.Invoke(_count);

            if (lastCount <= 0 && _count > 0)
            {
                OnContains?.Invoke();
            }
        }
    }
}