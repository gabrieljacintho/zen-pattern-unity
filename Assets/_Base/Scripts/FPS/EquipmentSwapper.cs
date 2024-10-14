using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Inventory;
using FireRingStudio.Save;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.FPS
{
    public class EquipmentSwapper : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private Equipment.Equipment _unarmedEquipment;
        [SerializeField] private bool _playUnequipAnimationOnUnarmed = true;

        private List<Equipment.Equipment> _equipments;
        
        private Equipment.Equipment _currentEquipment;
        private Equipment.Equipment _lastEquipment;
        private InventorySlot _lastSelectedInventorySlot;

        public List<Equipment.Equipment> Equipments
        {
            get
            {
                if (_equipments == null || _equipments.Count == 0)
                {
                    _equipments = GetComponentsInChildren<Equipment.Equipment>(true).ToList();
                }

                return _equipments;
            }
        }
        public Equipment.Equipment CurrentEquipment => _currentEquipment;
        private InventorySlot SelectedInventorySlot => _inventory != null ? _inventory.SelectedSlot : null;

        [Space]
        public UnityEvent<Equipment.Equipment> OnEquipmentChanged;


        private void OnEnable()
        {
            this.DoOnNextFrame(() =>
            {
                if (_inventory != null)
                {
                    _inventory.SelectedSlotChanged += UpdateEquipment;
                    _inventory.SlotChanged += UpdateEquipment;
                    _inventory.Reset += Restore;
                }

                GameManager.GameUnpaused += UpdateEquipment;

                UpdateEquipment(SelectedInventorySlot);
            });
        }

        private void OnDisable()
        {
            if (_inventory != null)
            {
                _inventory.SelectedSlotChanged -= UpdateEquipment;
                _inventory.SlotChanged -= UpdateEquipment;
                _inventory.Reset -= Restore;
            }

            GameManager.GameUnpaused -= UpdateEquipment;
        }

        private void Equip(Equipment.Equipment equipment, bool playUnequipFXs = false)
        {
            if (_currentEquipment == equipment)
            {
                return;
            }

            if (_lastEquipment != null)
            {
                _lastEquipment.gameObject.SetActive(false);
            }
            
            Unequip(playUnequipFXs);

            _currentEquipment = equipment != null ? equipment : _unarmedEquipment;
            
            if (_currentEquipment != null)
            {
                _currentEquipment.Equip();
            }
        }

        public void Unequip(bool playFXs = false)
        {
            if (_currentEquipment == null)
            {
                return;
            }

            if (playFXs)
            {
                this.DoOnNextFrame(() =>
                {
                    playFXs &= HasEquipmentInInventory(_lastEquipment);
                    _lastEquipment.Unequip(playFXs);
                });
            }
            else
            {
                _currentEquipment.Unequip();
            }

            _lastEquipment = _currentEquipment;
            _currentEquipment = null;
        }
        
        private void UpdateEquipment(InventorySlot slot)
        {
            if (GameManager.IsPaused || slot != SelectedInventorySlot)
            {
                return;
            }
            
            if (slot == null)
            {
                Unarmed();
                return;
            }
            
            Equipment.Equipment equipment = FindSlotEquipment(slot);
            if (equipment != null)
            {
                if (_lastSelectedInventorySlot != null && _lastSelectedInventorySlot != slot)
                {
                    Unequip();
                }
                Equip(equipment);
            }
            else
            {
                Unarmed();
            }
            
            _lastSelectedInventorySlot = slot;
            
            OnEquipmentChanged?.Invoke(_currentEquipment);
        }

        private void Unarmed()
        {
            if (_unarmedEquipment != null)
            {
                Equip(_unarmedEquipment, _playUnequipAnimationOnUnarmed);
            }
            else
            {
                Unequip(true);
            }
                
            OnEquipmentChanged?.Invoke(_currentEquipment);
        }

        public void UpdateEquipment()
        {
            if (_inventory != null)
            {
                UpdateEquipment(_inventory.SelectedSlot);
            }
        }

        public Equipment.Equipment FindSlotEquipment(InventorySlot slot)
        {
            return slot.IsEmpty ? null : Equipments?.Find(equipment => equipment.Data == slot.ItemData);
        }

        public bool HasEquipmentInInventory(Equipment.Equipment equipment)
        {
            return _inventory != null && equipment.Data != null && _inventory.HasItem(equipment.Data);
        }
                
        public void Restore()
        {
            Equipments?.ForEach(equipment => equipment.ResetToDefault());
            _currentEquipment = null;
            _lastSelectedInventorySlot = null;
            
            UpdateEquipment(SelectedInventorySlot);
        }
    }
}