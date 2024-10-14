using FireRingStudio.Cache;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FireRingStudio.Inventory.UI
{
    public class InventorySlotDisplay : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private string _slotId;
        [SerializeField] private Button _button;

        private InventorySlot _slot;
        private ItemPackDisplay _itemPackDisplay;

        private bool _subscribed;
        private bool _buttonGot;

        public Button Button
        {
            get
            {
                if (!_buttonGot && _button == null)
                {
                    _button = GetComponent<Button>();
                    _buttonGot = true;
                }

                return _button;
            }
        }
        public InventorySlot Slot
        {
            get
            {
                if (_inventory != null && (_slot == null || !_inventory.Slots.Contains(_slot)))
                {
                    Slot = _inventory.FindSlotWithID(_slotId);
                    _subscribed = false;
                }

                if (!_subscribed && Slot != null)
                {
                    Slot.Changed += UpdateDisplay;
                    _subscribed = true;
                }

                return _slot;
            }
            private set => Initialize(value);
        }
        public ItemPackDisplay ItemPackDisplay
        {
            get
            {
                if (_itemPackDisplay == null)
                {
                    _itemPackDisplay = GetComponentInChildren<ItemPackDisplay>(true);
                }

                return _itemPackDisplay;
            }
            private set => _itemPackDisplay = value;
        }
        
        [Space]
        public UnityEvent OnEmptied;
        public UnityEvent OnFilled;
        
        
        private void OnEnable()
        {
            UpdateDisplay();
        }

        private void OnDisable()
        {
            if (Slot != null)
            {
                Slot.Changed -= UpdateDisplay;
                _subscribed = false;
            }
        }

        public void Initialize(InventorySlot slot)
        {
            if (_slot == slot)
            {
                return;
            }
            
            if (_slot != null && _subscribed)
            {
                _slot.Changed -= UpdateDisplay;
            }

            _slot = slot;
            _inventory = _slot?.SourceInventory;
            _slotId = _slot?.ID;
            
            if (_slot != null)
            {
                _slot.Changed += UpdateDisplay;
                _subscribed = true;
            }
            
            UpdateDisplay();
        }
        
        public bool ExchangeItemPack(InventorySlotDisplay otherSlotDisplay)
        {
            if (!CanExchangeItemPack(otherSlotDisplay, out ItemDraggableGraphic draggableGraphic, out ItemDraggableGraphic otherDraggableGraphic))
            {
                return false;
            }
            
            InventorySlot otherSlot = otherSlotDisplay.Slot;
            InventoryCanvas otherInventoryCanvas = otherDraggableGraphic.InventoryCanvas;
            InventorySlotDisplay otherInventorySlotDisplay = otherDraggableGraphic.InventorySlotDisplay;

            ItemPackDisplay itemPackDisplay = ItemPackDisplay;
            
            otherDraggableGraphic.InventoryCanvas = draggableGraphic.InventoryCanvas;
            otherDraggableGraphic.InventorySlotDisplay = draggableGraphic.InventorySlotDisplay;
            ItemPackDisplay = otherSlotDisplay.ItemPackDisplay;
            
            draggableGraphic.InventoryCanvas = otherInventoryCanvas;
            draggableGraphic.InventorySlotDisplay = otherInventorySlotDisplay;
            otherSlotDisplay.ItemPackDisplay = itemPackDisplay;
            
            (otherSlot.ItemPack, Slot.ItemPack) = (Slot.ItemPack, otherSlot.ItemPack);

            return true;
        }

        public bool ExchangeItemPackDisplay(InventorySlotDisplay otherSlotDisplay)
        {
            if (!CanExchangeItemPack(otherSlotDisplay, out ItemDraggableGraphic draggableGraphic, out ItemDraggableGraphic otherDraggableGraphic))
            {
                return false;
            }
            
            Transform otherDefaultParent = otherDraggableGraphic.DefaultParent;
            int otherDefaultSiblingIndex = otherDraggableGraphic.DefaultSiblingIndex;
            
            otherDraggableGraphic.DefaultParent = draggableGraphic.DefaultParent;
            otherDraggableGraphic.DefaultSiblingIndex = draggableGraphic.DefaultSiblingIndex;
            otherDraggableGraphic.Detach();

            draggableGraphic.DefaultParent = otherDefaultParent;
            draggableGraphic.DefaultSiblingIndex = otherDefaultSiblingIndex;
            draggableGraphic.Detach();

            return true;
        }
        
        public void InvokeEvents()
        {
            if (Slot != null && !Slot.IsEmpty)
            {
                OnFilled?.Invoke();
            }
            else
            {
                OnEmptied?.Invoke();
            }
        }

        private bool CanExchangeItemPack(InventorySlotDisplay otherSlotDisplay, out ItemDraggableGraphic draggableGraphic, out ItemDraggableGraphic otherDraggableGraphic)
        {
            draggableGraphic = null;
            otherDraggableGraphic = null;
            
            ItemPackDisplay itemPackDisplay = ItemPackDisplay;
            ItemPackDisplay otherItemPackDisplay = otherSlotDisplay.ItemPackDisplay;
            if (itemPackDisplay == null || otherItemPackDisplay == null)
            {
                return false;
            }
            
            if (otherSlotDisplay.Slot == null || !CanExchangeItemPack(otherSlotDisplay.Slot))
            {
                return false;
            }
            
            draggableGraphic = ComponentCacheManager.GetComponent<ItemDraggableGraphic>(itemPackDisplay.gameObject);
            otherDraggableGraphic = ComponentCacheManager.GetComponent<ItemDraggableGraphic>(otherItemPackDisplay.gameObject);
            if (draggableGraphic == null || otherDraggableGraphic == null)
            {
                return false;
            }

            return true;
        }
        
        private bool CanExchangeItemPack(InventorySlot otherSlot)
        {
            if (Slot == null || Slot.SourceInventory == null || otherSlot.SourceInventory == null)
            {
                return false;
            }
            
            return Slot.IsEmpty || otherSlot.IsValidItemPack(Slot.ItemPack);
        }

        private void UpdateDisplay()
        {
            UpdateItemPackDisplay();
            InvokeEvents();
        }

        private void UpdateItemPackDisplay()
        {
            if (ItemPackDisplay == null)
            {
                return;
            }

            ItemPack itemPack = Slot?.ItemPack;
            ItemPackDisplay.Initialize(itemPack);
        }
    }
}