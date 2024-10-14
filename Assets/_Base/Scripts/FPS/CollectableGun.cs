using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.Inventory;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio.FPS
{
    public class CollectableGun : CollectableItem
    {
        [Space]
        [SerializeField, ES3NonSerializable] private Vector2 _ammoClipFillAmountRange = new(0.5f, 1f);

        [ES3Serializable] private int _ammoClipSize = -1;


        protected override void Awake()
        {
            base.Awake();

            if (TryGetComponent(out PooledObject pooledObject))
            {
                pooledObject.OnGetAction += OnGet;
            }
        }
        
        public override bool TryCollect(InventoryData inventory)
        {
            if (!base.TryCollect(inventory))
            {
                return false;
            }

            TryPickUpAmmoClip();

            return true;
        }

        public override void OnDrop()
        {
            base.OnDrop();

            if (Data != null && Data is ProjectileWeaponData projectileWeaponData)
            {
                _ammoClipSize = projectileWeaponData.AmmoClip.Size;
            }
        }

        private bool TryPickUpAmmoClip()
        {
            if (!Interactable || Data == null || Data is not ProjectileWeaponData projectileWeaponData)
            {
                return false;
            }

            projectileWeaponData.AmmoClip.Size = _ammoClipSize < 0 ? GetRandomAmmoClipSize(projectileWeaponData) : _ammoClipSize;
            _ammoClipSize = -1;
            
            return true;
        }
        
        private int GetRandomAmmoClipSize(ProjectileWeaponData projectileWeaponData)
        {
            if (projectileWeaponData.AmmoClip == null)
            {
                return 0;
            }
            
            float percentage = UnityEngine.Random.Range(_ammoClipFillAmountRange.x, _ammoClipFillAmountRange.y);
            float value = Mathf.Lerp(0, projectileWeaponData.AmmoClip.MaxSize, percentage);
            
            return Mathf.RoundToInt(value);
        }

        private void OnGet()
        {
            _ammoClipSize = -1;
        }
    }
}