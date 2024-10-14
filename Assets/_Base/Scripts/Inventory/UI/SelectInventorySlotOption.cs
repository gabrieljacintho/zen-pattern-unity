namespace FireRingStudio.Inventory.UI
{
    public class SelectInventorySlotOption : InventorySlotOption
    {
        public override void Execute()
        {
            InventorySlot.Select();
        }

        public override bool IsAvailable()
        {
            return base.IsAvailable() && InventorySlot.SourceInventory != null;
        }

        protected override bool IsAvailable(ItemData itemData)
        {
            return itemData.CanBeSelected;
        }
    }
}