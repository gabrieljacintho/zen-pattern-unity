using UnityEngine;

namespace FireRingStudio.Inventory.UI
{
    public class DiscardItemPackOption : InventorySlotOption
    {
        [SerializeField] private string _itemDropperId;

        private ItemDropper _itemDropper;
        
        private ItemDropper ItemDropper
        {
            get
            {
                if (_itemDropper == null)
                {
                    _itemDropper = ComponentID.FindComponentWithID<ItemDropper>(_itemDropperId);
                }

                return _itemDropper;
            }
        }
        
        
        public override void Execute()
        {
            ItemDropper.DropAll(InventorySlot.ItemPack);
        }

        public override bool IsAvailable()
        {
            return base.IsAvailable() && ItemDropper != null;
        }

        protected override bool IsAvailable(ItemData itemData)
        {
            return itemData.CanBeDiscarded;
        }
    }
}