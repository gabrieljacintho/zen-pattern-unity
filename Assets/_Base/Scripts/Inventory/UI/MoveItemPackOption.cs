using FireRingStudio.Cache;

namespace FireRingStudio.Inventory.UI
{
    public class MoveItemPackOption : InventorySlotOption
    {
        private ItemDraggableGraphic _itemDraggableGraphic;

        public override bool IsExecuting => _itemDraggableGraphic != null && _itemDraggableGraphic.IsMoving;
        private ItemPackDisplay ItemPackDisplay => InventorySlotDisplay != null ? InventorySlotDisplay.ItemPackDisplay : null;
        private ItemDraggableGraphic ItemDraggableGraphic => ItemPackDisplay != null ?
            ComponentCacheManager.GetComponent<ItemDraggableGraphic>(ItemPackDisplay.gameObject) : null;
        
        
        public override void Execute()
        {
            _itemDraggableGraphic = ItemDraggableGraphic;
            _itemDraggableGraphic.StartMoving();
        }

        public override void ConfirmChanges()
        {
            _itemDraggableGraphic.ConfirmMovement();
            _itemDraggableGraphic = null;
        }

        public override void CancelChanges()
        {
            _itemDraggableGraphic.CancelMovement();
            _itemDraggableGraphic = null;
        }

        public override bool IsAvailable()
        {
            return base.IsAvailable() && ItemDraggableGraphic != null;
        }

        protected override bool IsAvailable(ItemData itemData)
        {
            return itemData.CanBeMoved;
        }
    }
}