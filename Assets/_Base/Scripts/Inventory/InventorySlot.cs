using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private string _id;
        [SerializeField] private StringConstant _category;
        [SerializeField] private ItemPack _itemPack;
        [SerializeField] private bool _canBeReplaced;

        [ES3Serializable] private InventoryData _sourceInventory;
        
        public string ID => _id;
        public StringConstant Category => _category;
        public ItemPack ItemPack
        {
            get => _itemPack;
            set => SetItemPack(value);
        }
        public bool CanBeReplaced => _canBeReplaced;
        public InventoryData SourceInventory
        {
            get => _sourceInventory;
            set => _sourceInventory = value;
        }

        public ItemData ItemData => ItemPack?.Data;
        public bool IsEmpty => ItemPack == null || ItemPack.Data == null || ItemPack.IsEmpty;
        public bool IsFull => ItemPack != null && ItemPack.IsFull;
        public bool IsSelected => _sourceInventory != null && _sourceInventory.SelectedSlot == this;
        
        public delegate void InventorySlotEvent(); 
        public event InventorySlotEvent Changed;


        public InventorySlot(InventoryData sourceInventory, string id, StringConstant category, ItemPack itemPack, bool canBeReplaced = false)
        {
            _sourceInventory = sourceInventory;
            _id = id;
            _category = category;
            if (itemPack == null || !IsValidItemPack(itemPack) || !SetItemPack(itemPack))
            {
                _itemPack = null;
            }
            _canBeReplaced = canBeReplaced;
        }
        
        public InventorySlot(InventoryData sourceInventory, InventorySlot inventorySlot) : this(sourceInventory, inventorySlot.ID,
            inventorySlot.Category, inventorySlot.ItemPack != null ? new ItemPack(inventorySlot.ItemPack) : null, inventorySlot.CanBeReplaced) { }

        public int AddItem(ItemData data, int quantity = 1)
        {
            if (data == null || !IsValidItemData(data))
            {
                Debug.LogError("Invalid item data!");
                return 0;
            }

            if (!IsEmpty && data != ItemData)
            {
                return 0;
            }

            int result = 0;
            if (IsEmpty)
            {
                if (SetItemPack(new ItemPack(data, quantity)))
                {
                    result = ItemPack.Size;
                }
            }
            else
            {
                result = ItemPack.IncreaseSize(quantity);
            }

            if (result > 0)
            {
                Changed?.Invoke();
            }

            return result;
        }
        
        public int AddItem(int quantity = 1)
        {
            return ItemData != null ? AddItem(ItemData, quantity) : 0;
        }
        
        public int RemoveItem(int quantity = 1)
        {
            int result = 0;
            if (IsEmpty)
            {
                return result;
            }
            
            result = ItemPack.DecreaseSize(quantity);

            if (IsEmpty)
            {
                _itemPack = null;
            }
            
            Changed?.Invoke();

            return result;
        }

        public void Clear()
        {
            _itemPack = null;
            
            Changed?.Invoke();
        }

        public void Select()
        {
            if (_sourceInventory != null)
            {
                _sourceInventory.SelectedSlot = this;
            }
        }
        
        public bool SetItemPack(ItemPack value)
        {
            if (value != null && !IsValidItemPack(value))
            {
                Debug.LogError("Invalid item pack!");
                return false;
            }
            
            if (_itemPack == value)
            {
                return true;
            }

            void InvokeChangedEvent()
            {
                Changed?.Invoke();
            }
            
            if (_itemPack != null)
            {
                _itemPack.Changed -= InvokeChangedEvent;
            }

            _itemPack = value;
            
            if (_itemPack != null)
            {
                _itemPack.Changed += InvokeChangedEvent;
            }
            
            Changed?.Invoke();
            
            return true;
        }

        public bool IsValidItemData(ItemData itemData)
        {
            return itemData.Category == _category;
        }
        
        public bool IsValidItemPack(ItemPack itemPack)
        {
            return itemPack.IsEmpty || (itemPack.Data != null && IsValidItemData(itemPack.Data));
        }

        /*public static bool operator ==(InventorySlot slot1, InventorySlot slot2)
        {
            if (slot1 is null)
            {
                return slot2 is null;
            }

            if (slot1.Equals(slot2))
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(InventorySlot b1, InventorySlot b2)
        {
            return !(b1 == b2);
        }*/

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }

            if (obj != null && !string.IsNullOrEmpty(_id) && obj is InventorySlot slot2)
            {
                return _id == slot2.ID;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}