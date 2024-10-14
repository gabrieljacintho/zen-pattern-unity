using System.Collections.Generic;
using FireRingStudio.FPS;
using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.Inventory;
using UnityEngine;

namespace FireRingStudio.Random
{
    public class DynamicAmmoWeights : DynamicItemWeights
    {
        protected override void UpdateWeights()
        {
            if (_itemWeights == null)
            {
                return;
            }
            
            List<ItemData> equippedAmmoDataset = GetEquippedAmmoDataset();
            if (equippedAmmoDataset == null)
            {
                return;
            }
            
            foreach (ItemWeight itemWeight in _itemWeights)
            {
                bool isEquipped = itemWeight.ItemData != null && equippedAmmoDataset.Contains(itemWeight.ItemData);
                float t = isEquipped ? GetWeightFactor(itemWeight) : 0f;
                UpdateWeight(itemWeight, t);
            }
        }

        private List<ItemData> GetEquippedAmmoDataset()
        {
            if (_inventory == null)
            {
                return null;
            }
            
            List<ItemData> equippedAmmoDataset = new List<ItemData>();
            foreach (InventorySlot slot in _inventory.Slots)
            {
                if (slot.IsEmpty || slot.ItemPack.Data is not ProjectileWeaponData gunData)
                {
                    continue;
                }

                ItemData ammoData = gunData.AmmoClip?.Data;
                if (ammoData == null)
                {
                    continue;
                }
                
                equippedAmmoDataset.Add(ammoData);
            }

            return equippedAmmoDataset;
        }
    }
}