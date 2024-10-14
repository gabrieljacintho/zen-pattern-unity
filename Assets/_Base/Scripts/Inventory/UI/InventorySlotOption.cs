using FireRingStudio.UI.Options;
using UnityEngine;

namespace FireRingStudio.Inventory.UI
{
    [RequireComponent(typeof(InventorySlotDisplay))]
    public abstract class InventorySlotOption : SelectableOption
    {
        private InventorySlotDisplay _inventorySlotDisplay;

        protected InventorySlotDisplay InventorySlotDisplay
        {
            get
            {
                if (_inventorySlotDisplay == null)
                {
                    _inventorySlotDisplay = GetComponent<InventorySlotDisplay>();
                }

                return _inventorySlotDisplay;
            }
        }
        protected InventorySlot InventorySlot => InventorySlotDisplay != null ? InventorySlotDisplay.Slot : null;


        public override bool IsAvailable()
        {
            return InventorySlot != null && !InventorySlot.IsEmpty && IsAvailable(InventorySlot.ItemData);
        }

        protected abstract bool IsAvailable(ItemData itemData);
    }
}