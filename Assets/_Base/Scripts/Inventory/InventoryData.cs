using System;
using System.Collections.Generic;
using System.Linq;
using FireRingStudio.Extensions;
using FireRingStudio.Save;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    [CreateAssetMenu(menuName = "FireRing Studio/Inventory/Inventory Data", fileName = "New Inventory Data")]
    public class InventoryData : ScriptableObject, ISave
    {
        [ES3NonSerializable] public LocalizedString DisplayName;
        [ES3NonSerializable] public LocalizedString Description;
        
        [Space]
        [ES3NonSerializable] public List<InventorySlot> InitialSlots;
        [Tooltip("Set negative to not limit.")]
        [ES3NonSerializable] public List<KeyValue<StringConstant, int>> MaxSlotsByCategory;
        [Tooltip("Set negative to not limit.")]
        [ES3NonSerializable] public int DefaultMaxSlotsByCategory = -1;
        [Tooltip("Set negative to not limit.")]
        [ES3NonSerializable] public int MaxSlotsPerItem = -1;
	[ES3NonSerializable] public int InitialSelectedSlotIndex = 0;

        [Header("Save")]
        [SerializeField, ES3NonSerializable] private string _saveKey = "inventory";

        [ES3Serializable] private List<InventorySlot> _slots = new();
        [ES3Serializable] private InventorySlot _selectedSlot;

        [ES3NonSerializable] public List<InventorySlot> Slots => _slots;
        [ES3NonSerializable]
        public InventorySlot SelectedSlot
        {
            get => _selectedSlot;
            set
            {
                if (_selectedSlot != value)
                {
                    _selectedSlot = value;
                    SelectedSlotChanged?.Invoke(value);
                }
            }
        }

        [ES3NonSerializable] public List<ItemPack> ItemPacks => GetItemPacks();

        public string SaveKey => _saveKey;

        private string SlotsSaveKey => _saveKey + "-slots";
        private string SelectedSlotIndexSaveKey => _saveKey + "-selected-slot-index";

        public delegate void InventoryEvent();
        public event InventoryEvent Changed;
        public event InventoryEvent Reset;
        public delegate void InventorySlotEvent(InventorySlot slot);
        public event InventorySlotEvent SlotChanged;
        public event InventorySlotEvent SelectedSlotChanged;


        public int AddItem(ItemData data, int quantity = 1)
        {
            if (MaxSlotsPerItem == 0)
            {
                Debug.LogError("Max slots per item is zero!", this);
                return 0;
            }

            int quantityAdded = 0;
            int remainingQuantity = quantity;

            List<InventorySlot> slots = _slots.FindAll(slot => !slot.IsEmpty && slot.ItemData == data);
            quantityAdded += AddItemToSlots(data, slots, remainingQuantity);
            remainingQuantity = quantity - quantityAdded;

            if (remainingQuantity == 0)
            {
                return quantityAdded;
            }

            int currentQuantityOfSlots = slots.Count;
            if (MaxSlotsPerItem > 0 && currentQuantityOfSlots >= MaxSlotsPerItem)
            {
                return quantityAdded;
            }

            slots = _slots.FindAll(slot => slot.Category == data.Category);
            quantityAdded += AddItemToSlots(data, slots, remainingQuantity);
            remainingQuantity = quantity - quantityAdded;

            if (remainingQuantity == 0)
            {
                return quantityAdded;
            }

            currentQuantityOfSlots = slots.Count;
            int maxQuantityOfSlots = GetMaxQuantityOfSlotsWithCategory(data.Category);

            while (currentQuantityOfSlots < maxQuantityOfSlots || maxQuantityOfSlots < 0)
            {
                InventorySlot slot = CreateEmptySlot(data.Category);
                currentQuantityOfSlots++;

                quantityAdded += slot.AddItem(data, remainingQuantity);
                if (quantityAdded == 0)
                {
                    Debug.LogError("Could not add item to empty slot!", data);
                    break;
                }

                remainingQuantity = quantity - quantityAdded;
                if (remainingQuantity == 0)
                {
                    break;
                }
            }
            
            return quantityAdded;
        }

        private int AddItemToSlots(ItemData data, List<InventorySlot> slots, int quantity = 1)
        {
            int quantityAdded = 0;
            int remainingQuantity = quantity;

            foreach (InventorySlot slot in slots)
            {
                quantityAdded += slot.AddItem(data, remainingQuantity);
                remainingQuantity = quantity - quantityAdded;
                if (remainingQuantity == 0)
                {
                    return quantityAdded;
                }
            }

            return quantityAdded;
        }

        public int RemoveItem(ItemData data, int quantity = 1)
        {
            List<InventorySlot> slots = _slots.FindAll(slot => slot.ItemData == data && !slot.IsEmpty);
            IEnumerable<InventorySlot> orderedSlots = slots.OrderBy(slot => slot.ItemPack?.Size ?? 0);
            
            int removedQuantity = 0;
            foreach (InventorySlot slot in orderedSlots)
            {
                int remainingQuantity = quantity - removedQuantity;
                removedQuantity += slot.RemoveItem(remainingQuantity);
                
                remainingQuantity = quantity - removedQuantity;
                if (remainingQuantity == 0)
                {
                    break;
                }
            }
            
            return removedQuantity;
        }

        public int GetQuantityOfItem(ItemData data)
        {
            List<InventorySlot> slots = _slots.FindAll(slot => slot.ItemData == data && slot.ItemPack != null);

            return slots.Sum(slot => slot.ItemPack.Size);
        }
        
        public int SetQuantityOfItem(ItemData data, int value)
        {
            int currentQuantity = GetQuantityOfItem(data);
            int remainingQuantity = value - currentQuantity;

            if (remainingQuantity > 0)
            {
                return currentQuantity + AddItem(data, remainingQuantity);
            }
            else if (remainingQuantity < 0)
            {
                return currentQuantity - RemoveItem(data, -remainingQuantity);
            }

            return currentQuantity;
        }
        
        public int GetMaxQuantityOfItem(ItemData data)
        {
            if (data.MaxPackSize == 0)
            {
                Debug.LogWarning("Max pack size is zero!", data);
                return 0;
            }

            int maxQuantityOfSlots = GetMaxQuantityOfSlotsWithCategory(data.Category);
            if (MaxSlotsPerItem >= 0)
            {
                if (maxQuantityOfSlots >= 0)
                {
                    maxQuantityOfSlots = Mathf.Min(maxQuantityOfSlots, MaxSlotsPerItem);
                }
                else
                {
                    maxQuantityOfSlots = MaxSlotsPerItem;
                }
            }

            return maxQuantityOfSlots * data.MaxPackSize;
        }
        
        public bool CanAddItem(ItemData itemData, int quantity = 1)
        {
            int maxQuantity = GetMaxQuantityOfItem(itemData);
            if (maxQuantity < 0)
            {
                return true;
            }
            
            return GetQuantityOfItem(itemData) + quantity <= maxQuantity;
        }

        #region Query Functions

        public List<ItemPack> GetItemPacks(StringConstant[] requiredCategories = null)
        {
            List<ItemPack> itemPacks = new List<ItemPack>();
            foreach (InventorySlot slot in _slots)
            {
                if (slot.IsEmpty)
                {
                    continue;
                }

                if (requiredCategories != null && requiredCategories.Length > 0)
                {
                    if (!requiredCategories.Contains(slot.ItemData.Category))
                    {
                        continue;
                    }
                }

                itemPacks.Add(slot.ItemPack);
            }

            return itemPacks;
        }

        public List<ItemPack> GetItemPacksOfDataType<T>() where T : ItemData
        {
            List<ItemPack> itemPacks = new List<ItemPack>();
            foreach (InventorySlot slot in _slots)
            {
                if (slot.ItemPack != null && slot.ItemPack.Data is T)
                {
                    itemPacks.Add(slot.ItemPack);
                }
            }

            return itemPacks;
        }

        public InventorySlot FindSlotWithID(string id)
        {
            return _slots.Find(slot => slot.ID == id);
        }

        public bool HasItem(ItemData data)
        {
            return GetQuantityOfItem(data) > 0;
        }

        #endregion

        public void Restore()
        {
            _slots.Clear();

            CreateInitialSlots();
            CreateSlots();

            SelectedSlot = _slots.Count > InitialSelectedSlotIndex ? _slots[InitialSelectedSlotIndex] : null;
            
            Changed?.Invoke();
            Reset?.Invoke();
        }

        private int GetMaxQuantityOfSlotsWithCategory(StringConstant category)
        {
            if (MaxSlotsByCategory.TryGetValue(category, out int value))
            {
                return value;
            }

            return DefaultMaxSlotsByCategory;
        }

        #region Create Slots

        private void CreateInitialSlots()
        {
            if (InitialSlots == null)
            {
                return;
            }
            
            foreach (InventorySlot slot in InitialSlots)
            {
                CreateSlot(slot, false);
            }
        }

        private void CreateSlots()
        {
            if (MaxSlotsByCategory == null)
            {
                return;
            }
            
            foreach (KeyValue<StringConstant, int> maxSlotsByCategory in MaxSlotsByCategory)
            {
                if (maxSlotsByCategory.Value < 1)
                {
                    continue;
                }
                    
                int count = _slots.FindAll(slot => slot.Category == maxSlotsByCategory.Key).Count;
                for (int i = count; i < maxSlotsByCategory.Value; i++)
                {
                    CreateEmptySlot(maxSlotsByCategory.Key, false);
                }
            }
        }

        private InventorySlot CreateSlot(InventorySlot slot, bool invokeEvents = true)
        {
            InventorySlot newSlot = FindSlotWithID(slot.ID);
            if (newSlot == null)
            {
                newSlot = new InventorySlot(this, slot);
                AddSlot(newSlot, invokeEvents);
            }

            return newSlot;
        }

        private InventorySlot CreateEmptySlot(StringConstant category, bool invokeEvents = true)
        {
            InventorySlot slot = new InventorySlot(this, null, category, null);
            AddSlot(slot, invokeEvents);

            return slot;
        }

        private void AddSlot(InventorySlot slot, bool invokeEvents = true)
        {
            if (_slots.Contains(slot))
            {
                return;
            }

            _slots.Add(slot);

            slot.Changed += () => SlotChanged?.Invoke(slot);
            slot.Changed += () => Changed?.Invoke();

            if (invokeEvents)
            {
                SlotChanged?.Invoke(slot);
                Changed?.Invoke();
            }
        }

        #endregion

        #region Save

        public void Save(ES3Settings settings)
        {
            ES3.Save(SaveKey, true, settings);
            ES3.Save(SlotsSaveKey, _slots, settings);

            int selectedSlotIndex = _selectedSlot != null && _slots.Contains(_selectedSlot) ? _slots.IndexOf(_selectedSlot) : 0;
            ES3.Save(SelectedSlotIndexSaveKey, selectedSlotIndex, settings);
        }

        [Button]
        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            SaveManager.RegisterSave(this);

            if (!ES3.KeyExists(SaveKey))
            {
                Restore();
                return;
            }

            _slots.Clear();

            List<InventorySlot> slots = ES3.Load<List<InventorySlot>>(SlotsSaveKey, settings);
            foreach (InventorySlot slot in slots)
            {
                CreateSlot(slot, false);
            }

            int selectedSlotIndex = ES3.Load<int>(SelectedSlotIndexSaveKey, settings);
            SelectedSlot = _slots.Count > 0 ? _slots[selectedSlotIndex] : null;

            Changed?.Invoke();
            Reset?.Invoke();
        }

        [Button]
        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(SaveKey, settings);
            ES3.DeleteKey(SlotsSaveKey, settings);
            ES3.DeleteKey(SelectedSlotIndexSaveKey, settings);
        }

        [Button]
        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));

        #endregion
    }
}