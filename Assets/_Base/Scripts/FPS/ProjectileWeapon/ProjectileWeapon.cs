using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.FPS.ThrowingWeapon;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    public class ProjectileWeapon : ProjectileWeaponBase
    {
        private ThrowingWeaponFire _throwingWeaponFire;

        private ProjectileData AmmoData => Data != null ? Data.ProjectileData : null;


        protected override void Awake()
        {
            base.Awake();
            _throwingWeaponFire = GetComponent<ThrowingWeaponFire>();
        }

        protected override void Setup()
        {
            base.Setup();
            gameObject.GetOrAddComponent<ThrowingWeaponFire>();
        }
        
        protected override void FireProjectile()
        {
            if (_throwingWeaponFire == null || AmmoData == null || AmmoData.ProjectilePrefab == null)
            {
                return;
            }

            GameObject prefab = AmmoData.ProjectilePrefab;
            ThrowParameters parameters = Data.ThrowParameters;
            float precisionRadius = CurrentPrecision.Radius;
            float maxDistance = Data.MaxDistance;
            
            PooledObject projectileInstance = _throwingWeaponFire.ThrowProjectile(prefab, parameters, precisionRadius, maxDistance);
            
            Projectile projectile = ComponentCacheManager.GetComponent<Projectile>(projectileInstance.gameObject);
            if (projectile != null)
            {
                projectile.Initialize(Data);
            }
        }
    }
}