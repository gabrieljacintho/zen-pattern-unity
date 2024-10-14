using FireRingStudio.Cache;
using FireRingStudio.FPS.ThrowingWeapon;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    public class ItemDropper : MonoBehaviour
    {
        [SerializeField] private ThrowObject _throwObject;
        

        public int Drop(InventoryData inventoryData, ItemData itemData, int quantity = 1)
        {
            if (_throwObject == null || itemData.Prefab == null)
            {
                return 0;
            }
            
            quantity = inventoryData.RemoveItem(itemData, quantity);
            
            for (int i = 0; i < quantity; i++)
            {
                Throw(_throwObject, itemData.Prefab);
            }

            return quantity;
        }

        public int DropAll(InventoryData inventoryData, ItemData itemData)
        {
            int quantity = inventoryData.GetQuantityOfItem(itemData);
            return quantity > 0 ? Drop(inventoryData, itemData, quantity) : 0;
        }

        public int Drop(ItemPack itemPack, int quantity = 1)
        {
            ItemData itemData = itemPack.Data;
            if (_throwObject == null)
            {
                Debug.LogNull(nameof(ThrowObject), this);
                return 0;
            }

            if (itemData == null || itemData.Prefab == null)
            {
                Debug.LogError("Item data has no prefab!", this);
                return 0;
            }
            
            quantity = itemPack.DecreaseSize(quantity);
            
            for (int i = 0; i < quantity; i++)
            {
                Throw(_throwObject, itemData.Prefab);
            }

            return quantity;
        }
        
        public int DropAll(ItemPack itemPack)
        {
            int quantity = itemPack.Size;
            return quantity > 0 ? Drop(itemPack, quantity) : 0;
        }

        private static CollectableItem Throw(ThrowObject throwObject, GameObject prefab)
        {
            PooledObject instance = throwObject.Throw(prefab);
            CollectableItem collectableItem = ComponentCacheManager.GetComponent<CollectableItem>(instance.gameObject);
            if (collectableItem != null)
            {
                collectableItem.OnDrop();
            }

            return collectableItem;
        }
    }
}