using System;
using FireRingStudio.UI;
using UnityEngine.EventSystems;

namespace FireRingStudio.Inventory.UI
{
    public class ItemDraggableGraphic : DraggableGraphic
    {
        private InventoryCanvas _inventoryCanvas;
        private InventorySlotDisplay _inventorySlotDisplay;
        
        private InventorySlotDisplay _exchangedInventorySlotDisplay;
        
        private bool _isMoving;
        private bool _initialized;

        public InventoryCanvas InventoryCanvas
        {
            get => _inventoryCanvas;
            set => _inventoryCanvas = value;
        }
        public InventorySlotDisplay InventorySlotDisplay
        {
            get => _inventorySlotDisplay;
            set => _inventorySlotDisplay = value;
        }
        public bool IsMoving => _isMoving;
        protected override bool CanAutoAttach => base.CanAutoAttach && !IsMoving;


        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (!IsDragging && !IsMoving)
            {
                return;
            }

            MoveToSelectedSlotDisplay();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            CancelMovement();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            ConfirmMovement();
            base.OnEndDrag(eventData);
        }

        public void StartMoving()
        {
            _isMoving = true;
        }
        
        public void ConfirmMovement()
        {
            if (MoveToSelectedSlotDisplay())
            {
                ConfirmExchange();
            }

            _exchangedInventorySlotDisplay = null;
            _isMoving = false;
        }
        
        public void CancelMovement()
        {
            CancelExchange();
            _isMoving = false;
        }

        public override void Detach()
        {
            Initialize();
            base.Detach();
        }
        
        private bool Move(InventorySlotDisplay targetSlotDisplay)
        {
            if (targetSlotDisplay == _exchangedInventorySlotDisplay)
            {
                return true;
            }
            
            CancelExchange();
            
            if (InventorySlotDisplay == null || targetSlotDisplay == null || !targetSlotDisplay.ExchangeItemPackDisplay(InventorySlotDisplay))
            {
                return false;
            }
            
            _exchangedInventorySlotDisplay = targetSlotDisplay;

            if (_exchangedInventorySlotDisplay.Button != null)
            {
                _exchangedInventorySlotDisplay.Button.onClick.AddListener(ConfirmMovement);
            }

            return true;
        }

        private bool MoveToSelectedSlotDisplay()
        {
            InventorySlotDisplay selectedSlotDisplay = InventoryCanvas != null ? InventoryCanvas.SelectedInventorySlotDisplay : null;
            return Move(selectedSlotDisplay);
        }
        
        private void ConfirmExchange()
        {
            if (_exchangedInventorySlotDisplay != null)
            {
                _exchangedInventorySlotDisplay.ExchangeItemPack(InventorySlotDisplay);
            }
        }
        
        private void CancelExchange()
        {
            if (_exchangedInventorySlotDisplay == null)
            {
                return;
            }
            
            _exchangedInventorySlotDisplay.ExchangeItemPackDisplay(InventorySlotDisplay);
                
            if (_exchangedInventorySlotDisplay.Button != null)
            {
                _exchangedInventorySlotDisplay.Button.onClick.RemoveListener(ConfirmMovement);
            }
            
            _exchangedInventorySlotDisplay = null;
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            
            _inventoryCanvas = GetComponentInParent<InventoryCanvas>();
            _inventorySlotDisplay = GetComponentInParent<InventorySlotDisplay>();
            
            _initialized = true;
        }
    }
}