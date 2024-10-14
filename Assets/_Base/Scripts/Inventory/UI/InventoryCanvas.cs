using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.Inventory.UI
{
    [RequireComponent(typeof(Canvas))]
    public class InventoryCanvas : MonoBehaviour
    {
        private List<InventoryDisplay> _inventoryDisplays;
        private List<InventorySlotDisplay> _inventorySlotDisplays;

        public List<InventoryDisplay> InventoryDisplays
        {
            get
            {
                if (_inventoryDisplays == null)
                {
                    _inventoryDisplays = GetComponentsInChildren<InventoryDisplay>(true).ToList();
                }

                return _inventoryDisplays;
            }
        }
        public List<InventorySlotDisplay> InventorySlotDisplays => GetInventorySlotDisplays();
        public InventorySlotDisplay SelectedInventorySlotDisplay => GetSelectedInventorySlotDisplay();


        private InventorySlotDisplay GetSelectedInventorySlotDisplay()
        {
            GameObject selectedObject = EventManager.MainSelectedObject;
            return selectedObject != null ? InventorySlotDisplays.Find(slotPanel => slotPanel.gameObject == selectedObject) : null;
        }

        private List<InventorySlotDisplay> GetInventorySlotDisplays()
        {
            if (_inventorySlotDisplays == null)
            {
                _inventorySlotDisplays = GetComponentsInChildren<InventorySlotDisplay>(true).ToList();
            }
            
            foreach (InventoryDisplay inventoryDisplay in InventoryDisplays)
            {
                _inventorySlotDisplays.AddRangeUncontained(inventoryDisplay.SlotDisplays);
            }

            return _inventorySlotDisplays;
        }
    }
}