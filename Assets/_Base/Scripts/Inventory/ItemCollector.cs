using FireRingStudio.Interaction;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    public class ItemCollector : InteractorGeneric<CollectableItem>
    {
        [Header("Item Collector")]
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private bool _canReplace;
        [ShowIf("_canReplace")]
        [SerializeField] private ItemDropper _replacedItemDropper;
        
        [Header("Description")]
        [SerializeField] protected LocalizedString _collectDescription = "PickUp";
        [SerializeField] protected LocalizedString _cannotCollectDescription = "InventoryFull";
        [SerializeField] protected bool _addItemNameToDescription = true;
        
        
        public override bool TryInteractWith(CollectableItem collectableItem)
        {
            if (collectableItem.TryCollect(_inventory))
            {
                return true;
            }

            return _canReplace && collectableItem.TryReplace(_inventory, _replacedItemDropper);
        }
        
        protected override string GetInteractionDescription(CollectableItem collectableItem)
        {
            string interactionDescription;
            bool addItemName = false;
            if (CanInteractWith(collectableItem))
            {
                interactionDescription = _collectDescription;
                addItemName = _addItemNameToDescription;
            }
            else
            {
                interactionDescription = _cannotCollectDescription;
            }
            
            string description = collectableItem.Description;
            if (addItemName && !string.IsNullOrEmpty(description))
            {
                interactionDescription += " " + description;
            }
            
            return interactionDescription;
        }

        public override bool CanInteract()
        {
            return base.CanInteract() && _inventory != null;
        }

        public override bool CanInteractWith(CollectableItem collectableItem)
        {
            if (!base.CanInteractWith(collectableItem))
            {
                return false;
            }

            if (_inventory.CanAddItem(collectableItem.Data))
            {
                return true;
            }

            if (_canReplace && collectableItem.GetReplaceableItemPack(_inventory) != null)
            {
                return true;
            }
            
            return false;
        }
    }
}