using System;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Inventory.UI
{
    public class InventoryItemImages : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private StringConstant[] _requiredCategories;

        private Image[] _images;


        private void Awake()
        {
            _images = GetComponentsInChildren<Image>(true);
        }

        private void OnEnable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateImages;
            }
            UpdateImages();
        }
        
        private void OnDisable()
        {
            if (_inventory != null)
            {
                _inventory.Changed += UpdateImages;
            }
        }

        public void UpdateImages()
        {
            ResetImages();
            
            if (_inventory == null)
            {
                return;
            }
            
            List<ItemPack> itemPacks = _inventory.GetItemPacks(_requiredCategories);

            int i = 0;
            foreach (ItemPack itemPack in itemPacks)
            {
                for (int n = 0; n < itemPack.Size; n++)
                {
                    if (_images == null || i >= _images.Length)
                    {
                        Debug.LogWarning("There are more items than images for them!", this);
                        return;
                    }

                    UpdateImage(_images[i], itemPack);
                    i++;
                }
            }
        }

        private void ResetImages()
        {
            if (_images != null)
            {
                Array.ForEach(_images, image => image.enabled = false);
            }
        }

        private static void UpdateImage(Image image, ItemPack item)
        {
            image.sprite = item.Data != null ? item.Data.Icon : null;
            image.enabled = image.sprite != null;
        }
    }
}