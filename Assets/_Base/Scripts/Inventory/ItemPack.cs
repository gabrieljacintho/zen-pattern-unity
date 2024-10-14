using System;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Inventory
{
    [Serializable]
    public class ItemPack
    {
        [SerializeField, InlineEditor] private ItemData _data;
        [SerializeField] private IntReference _sizeReference;
        [SerializeField] private IntReference _maxSizeReference;

        public ItemData Data => _data;
        public int Size
        {
            get => _sizeReference?.Value ?? 0;
            set => SetSize(value);
        }
        public int MaxSize
        {
            get => _maxSizeReference?.Value ?? 0;
            set => SetMaxSize(value);
        }

        public bool IsFull => Size == MaxSize;
        public bool IsEmpty => Size == 0;
        
        public delegate void ItemPackEvent();
        public event ItemPackEvent Changed;

        
        public ItemPack(ItemData data, int size = 0)
        {
            _data = data;
            _maxSizeReference = data != null ? data.MaxPackSizeReference : null;
            Size = size;
        }

        public ItemPack(ItemPack itemPack) : this(itemPack.Data, itemPack.Size) { }
        
        public int IncreaseSize(int value)
        {
            if (MaxSize >= 0)
            {
                value = Mathf.Min(value, MaxSize - Size);
            }

            Size += value;

            return value;
        }
        
        public int DecreaseSize(int value)
        {
            value = Mathf.Min(value, Size);

            Size -= value;

            return value;
        }

        public void SetSize(int value)
        {
            if (MaxSize >= 0)
            {
                value = Mathf.Min(value, MaxSize);
            }
            
            value = Mathf.Max(value, 0);

            if (value == Size)
            {
                return;
            }
            
            if (_sizeReference != null)
            {
                _sizeReference.Value = value;
            }
            else
            {
                _sizeReference = new IntReference(value);
            }
            
            Changed?.Invoke();
        }

        public void SetMaxSize(int value)
        {
            if (value == MaxSize)
            {
                return;
            }
            
            if (_maxSizeReference != null)
            {
                _maxSizeReference.Value = value;
            }
            else
            {
                _maxSizeReference = new IntReference(value);
            }
            
            Size = Size;
            
            Changed?.Invoke();
        }
        
        public static int ComparePackSize(ItemPack x, ItemPack y)
        {
            if (x.Size == y.Size)
            {
                return 0;
            }

            return x.Size > y.Size ? 1 : -1;
        }
    }
}