using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.Inventory;
using UnityEngine;

namespace FireRingStudio.FPS
{
    public class CollectableAmmoClip : CollectableItem
    {
        [Header("Ammo Clip")]
        [SerializeField, ES3NonSerializable] private ProjectileWeaponData _projectileWeaponData;
        [SerializeField, ES3NonSerializable] private Vector2 _fillAmountRange = new(0.5f, 1f);
        
        [ES3Serializable] private int _quantity = -1;

        public override int Quantity
        {
            get
            {
                if (_quantity < 0)
                {
                    _quantity = GetRandomQuantity();
                }
                
                return _quantity;
            }
        }
        
        
        protected override void OnCollectFunc()
        {
            base.OnCollectFunc();
            _quantity = -1;
        }
        
        private int GetRandomQuantity()
        {
            if (_projectileWeaponData == null || _projectileWeaponData.AmmoClip == null)
            {
                return 0;
            }
            
            float percentage = UnityEngine.Random.Range(_fillAmountRange.x, _fillAmountRange.y);
            float value = Mathf.Lerp(0, _projectileWeaponData.AmmoClip.MaxSize, percentage);
            
            return Mathf.RoundToInt(value);
        }
    }
}