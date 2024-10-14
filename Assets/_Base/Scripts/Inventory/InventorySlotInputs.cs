using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio.Inventory
{
    public class InventorySlotInputs : InventorySlotSelector
    {
        [Serializable]
        public struct SlotInput
        {
            public string SlotId;
            public InputActionReference SelectInput;
        }
        
        [SerializeField] private List<SlotInput> _slotInputs;


        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterEquipInputs();
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            UnregisterEquipInputs();
        }
        
        public bool TrySelectSlotWithID(string slotId)
        {
            if (!CanChangeSelectedSlot())
            {
                return false;
            }

            InventorySlot slot = _inventory.FindSlotWithID(slotId);
            if (slot == null)
            {
                Debug.LogError("Slot not found! (\"" + slotId + "\")", this);
                return false;
            }
            
            return TrySelectSlot(slot);
        }
        
        private void TrySelect(InputAction.CallbackContext context)
        {
            if (!CanChangeSelectedSlot() || _slotInputs == null)
            {
                return;
            }

            foreach (SlotInput slot in _slotInputs)
            {
                InputActionReference equipInput = slot.SelectInput;
                if (equipInput == null || equipInput.action != context.action)
                {
                    continue;
                }
                
                TrySelectSlotWithID(slot.SlotId);
                return;
            }
        }
        
        private void RegisterEquipInputs()
        {
            if (_slotInputs == null)
            {
                return;
            }

            foreach (SlotInput slotInput in _slotInputs)
            {
                InputActionReference equipInput = slotInput.SelectInput;
                if (equipInput != null)
                {
                    equipInput.action.performed += TrySelect;
                }
            }
        }
        
        private void UnregisterEquipInputs()
        {
            if (_slotInputs == null)
            {
                return;
            }

            foreach (SlotInput slotInput in _slotInputs)
            {
                InputActionReference equipInput = slotInput.SelectInput;
                if (equipInput != null)
                {
                    equipInput.action.performed -= TrySelect;
                }
            }
        }
    }
}