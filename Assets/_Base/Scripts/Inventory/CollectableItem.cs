using FireRingStudio.Extensions;
using FireRingStudio.Interaction;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Inventory
{
    public class CollectableItem : InteractableBase
    {
        [FormerlySerializedAs("data")]
        [SerializeField, InlineEditor, ES3NonSerializable] protected ItemData _data;
        [FormerlySerializedAs("_releaseToPoolOnPickUp")]
        [SerializeField, ES3NonSerializable] protected bool _releaseToPoolOnCollect = true;
	    [SerializeField, ES3NonSerializable] private bool _unparentWhenParentDisable;

        public override bool Interactable => base.Interactable && _data != null && Quantity > 0;
        public override string Description => _data != null ? _data.DisplayName : null;
        public ItemData Data => _data;
        public virtual int Quantity => 1;
        
        [Space]
        [FormerlySerializedAs("OnCollected")]
        [FormerlySerializedAs("OnPickUp")]
        [ES3NonSerializable] public UnityEvent OnCollect;


	    private void OnDisable()
        {
            if (_unparentWhenParentDisable && gameObject.activeSelf && UpdateManager.Instance != null)
            {
                UpdateManager.Instance.DoOnNextFrame(() => transform.parent = null);
            }
        }

        public virtual bool TryCollect(InventoryData inventory)
        {
            if (!Interactable || !inventory.CanAddItem(_data))
            {
                return false;
            }

            if (inventory.AddItem(_data, Quantity) == 0)
            {
                return false;
            }
            
            OnCollectFunc();

            return true;
        }

        public bool TryReplace(InventoryData inventory, ItemDropper replacedItemDropper = null)
        {
            if (!Interactable || inventory.GetMaxQuantityOfItem(_data) == 0)
            {
                return false;
            }
            
            ItemPack replaceableItem = GetReplaceableItemPack(inventory);
            if (replaceableItem == null)
            {
                return false;
            }
            
            if (replacedItemDropper != null)
            {
                replacedItemDropper.DropAll(replaceableItem);                       
            }
            else
            {
                replaceableItem.Size = 0;
            }
            
            return TryCollect(inventory);
        }

        public ItemPack GetReplaceableItemPack(InventoryData inventory)
        {
            StringConstant category = null;
            if (Data != null)
            {
                category = Data.Category;
            }

            bool CanBeReplaced(InventorySlot slot) => slot != null && slot.Category == category && !slot.IsEmpty && slot.ItemData != Data && slot.CanBeReplaced;

            InventorySlot slot = inventory.SelectedSlot;
            if (CanBeReplaced(slot))
            {
                return slot.ItemPack;
            }

            if (inventory != null)
            {
                slot = inventory.Slots.Find(CanBeReplaced);
            }

            return slot?.ItemPack;
        }

        public virtual void OnDrop()
        {

        }

        protected virtual void OnCollectFunc()
        {
            OnCollect?.Invoke();
            
            if (_releaseToPoolOnCollect)
            {
                gameObject.ReleaseToPool();
            }
        }
    }
}