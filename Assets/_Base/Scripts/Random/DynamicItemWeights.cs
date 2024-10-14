using System;
using System.Collections.Generic;
using FireRingStudio.Inventory;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public struct ItemWeight
    {
        public ItemData ItemData;
        public FloatVariable WeightVariable;
        public FloatReference MinWeightReference;
        public FloatReference MaxWeightReference;

        public float MinWeight => MinWeightReference?.Value ?? 0f;
        public float MaxWeight => MaxWeightReference?.Value ?? 0f;
    }
    
    public class DynamicItemWeights : MonoBehaviour
    {
        [SerializeField] protected InventoryData _inventory;
        [SerializeField] protected List<ItemWeight> _itemWeights;
        
        
        private void OnEnable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateWeights;
            }
            
            UpdateWeights();
        }
        
        private void OnDisable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateWeights;
            }
        }

        protected virtual void UpdateWeights()
        {
            if (_itemWeights == null)
            {
                return;
            }
            
            foreach (ItemWeight itemWeight in _itemWeights)
            {
                float t = GetWeightFactor(itemWeight);
                UpdateWeight(itemWeight, t);
            }
        }

        protected static void UpdateWeight(ItemWeight itemWeight, float weightFactor)
        {
            FloatVariable weightVariable = itemWeight.WeightVariable;
            if (weightVariable == null)
            {
                return;
            }

            weightVariable.Value = Mathf.Lerp(itemWeight.MinWeight, itemWeight.MaxWeight, weightFactor);
        }

        protected float GetWeightFactor(ItemWeight itemWeight)
        {
            ItemData itemData = itemWeight.ItemData;
            if (_inventory == null || itemData == null)
            {
                return 0f;
            }

            int quantity = _inventory.GetQuantityOfItem(itemWeight.ItemData);
            int maxQuantity = _inventory.GetMaxQuantityOfItem(itemWeight.ItemData);

            if (maxQuantity < 0)
            {
                Debug.LogError("Item \"" + itemData.name + "\" has no max quantity!", itemData);
                return 0f;
            }
            
            if (maxQuantity == 0)
            {
                return 0f;
            }
            
            float t  = (float)quantity / maxQuantity;
            return Mathf.Abs(t - 1f);
        }
    }
}