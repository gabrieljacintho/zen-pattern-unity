using FireRingStudio.Extensions;
using FireRingStudio.FPS;
using FireRingStudio.FPS.Equipment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio.Inventory
{
    public abstract class InventorySlotSelector : MonoBehaviour
    {
        [SerializeField] protected InventoryData _inventory;
        [SerializeField] protected EquipmentSwapper _equipmentSwapper;
        [SerializeField] private InputActionReference _toggleSlotInput;

        [Header("Settings")]
        [SerializeField] private bool _autoSelectNewEquipments;
        [SerializeField] private bool _canSelectEmptySlot;
        
        private LimitedQueue<InventorySlot> _toggleableSlotsHistory = new LimitedQueue<InventorySlot>(6);

        private InventorySlot SelectedSlot
        {
            get => _inventory != null ? _inventory.SelectedSlot : null;
            set
            {
                if (_inventory != null)
                {
                    _inventory.SelectedSlot = value;
                }
            }
        }
        private Equipment CurrentEquipment => _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;


        protected virtual void OnEnable()
        {
            if (_toggleSlotInput != null)
            {
                _toggleSlotInput.action.performed += TryToggleSlot;
            }
            
            if (_inventory != null)
            {
                if (_inventory.SelectedSlot == null)
                {
                    TryToggleSlot();
                }
                
                _inventory.SlotChanged += UpdateSelectedSlot;
            }
        }
        
        protected virtual void OnDisable()
        {
            if (_toggleSlotInput != null)
            {
                _toggleSlotInput.action.performed -= TryToggleSlot;
            }
            
            if (_inventory != null)
            {
                _inventory.SlotChanged -= UpdateSelectedSlot;
            }
        }

        public bool TrySelectSlot(InventorySlot slot)
        {
            if (!CanChangeSelectedSlot() || !CanSelect(slot))
            {
                return false;
            }

            InventorySlot selectedSlot = _inventory.SelectedSlot;
            if (selectedSlot == slot)
            {
                return true;
            }
                        
            if (selectedSlot != _toggleableSlotsHistory[0] && CanToggle(selectedSlot))
            {
                _toggleableSlotsHistory.Enqueue(selectedSlot);
            }

            _inventory.SelectedSlot = slot;

            return true;
        }

        protected bool CanSelect(InventorySlot slot, out Equipment equipment)
        {
            equipment = null;
            
            if (slot == null)
            {
                return false;
            }

            if (slot.IsEmpty)
            {
                return _canSelectEmptySlot;
            }
            
            if (_equipmentSwapper != null)
            {
                equipment = _equipmentSwapper.FindSlotEquipment(slot);
                return equipment != null && equipment.CanEquip();
            }

            return true;
        }

        protected bool CanSelect(InventorySlot slot) => CanSelect(slot, out _);
        
        public bool TryToggleSlot()
        {
            if (!CanChangeSelectedSlot())
            {
                return false;
            }
            
            for (int i = 0; i < _toggleableSlotsHistory.Length; i++)
            {
                InventorySlot slot = _toggleableSlotsHistory[i];
                if (!_inventory.Slots.Contains(slot) || slot.Equals(SelectedSlot))
                {
                    continue;
                }
                
                if (CanToggle(slot) && TrySelectSlot(slot))
                {
                    return true;
                }
            }
            
            foreach (InventorySlot slot in _inventory.Slots)
            {
                if (_inventory.Slots.Contains(slot) && !slot.Equals(SelectedSlot) && CanToggle(slot) && TrySelectSlot(slot))
                {
                    return true;
                }
            }

            return false;
        }

        private void TryToggleSlot(InputAction.CallbackContext context)
        {
            TryToggleSlot();
        }

        protected bool CanToggle(InventorySlot slot, out Equipment equipment)
        {
            if (!CanSelect(slot, out equipment))
            {
                return false;
            }

            return equipment != null && CanToggle(equipment);
        }

        protected bool CanToggle(InventorySlot slot) => CanToggle(slot, out _);
        
        protected static bool CanToggle(Equipment equipment)
        {
            return equipment.Data != null && equipment.Data.CanAutoEquip;
        }
        
        protected bool CanChangeSelectedSlot()
        {
            return GameManager.InGame && _inventory != null && _inventory.Slots != null && (CurrentEquipment == null || !CurrentEquipment.InUse);
        }

        private void UpdateSelectedSlot(InventorySlot slot)
        {
            this.DoOnNextFrame(() =>
            {
                if (SelectedSlot == slot)
                {
                    if (slot.IsEmpty && !_canSelectEmptySlot)
                    {
                        TryToggleSlot();
                    }
                }
                else if (_autoSelectNewEquipments && GameManager.InGame && CanToggle(slot))
                {
                    TrySelectSlot(slot);
                }
            });
        }
        
        public void Restore()
        {
            _toggleableSlotsHistory.Clear();
        }
    }
}