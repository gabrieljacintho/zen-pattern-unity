using System.Collections.Generic;
using FireRingStudio.FPS;
using FireRingStudio.FPS.HealingItem;
using FireRingStudio.Inventory;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Random
{
    public class DynamicHealingItemWeights : DynamicItemWeights
    {
        [Header("Health")]
        [SerializeField] private FloatVariable _healthVariable;
        [SerializeField] private FloatVariable _maxHealthVariable;
        
        
        protected override void UpdateWeights()
        {
            if (_inventory == null || _itemWeights == null)
            {
                return;
            }
            
            List<ItemPack> healingItems = _inventory.GetItemPacksOfDataType<HealingItemData>();
            float healingAvailable = GetAvailableHealing(healingItems);
            float healingRequired = GetRequiredHealing();
            
            float t1 = 0f;
            if (healingRequired > 0f)
            {
                t1 = Mathf.Lerp(1f, 0f, healingAvailable / healingRequired);
            }
            
            foreach (ItemWeight itemWeight in _itemWeights)
            {
                float t2 = GetWeightFactor(itemWeight);
                UpdateWeight(itemWeight, t1 * t2);
            }
        }
        
        private float GetRequiredHealing()
        {
            float maxHealth = _maxHealthVariable != null ? _maxHealthVariable.Value : 0f;
            float health = _healthVariable != null ? _healthVariable.Value : 0f;
            
            return maxHealth - health;
        }
        
        private static float GetAvailableHealing(List<ItemPack> healingItemStacks)
        {
            float value = 0f;
            foreach (ItemPack itemStack in healingItemStacks)
            {
                value += GetAvailableHealing(itemStack);
            }
            
            return value;
        }

        private static float GetAvailableHealing(ItemPack healingItemPack)
        {
            HealingItemData healingItemData = (HealingItemData)healingItemPack.Data;
            float healing = Mathf.Lerp(healingItemData.MinHeal, healingItemData.MaxHeal, 0.5f);
            
            return healing * healingItemPack.Size;
        }
    }
}