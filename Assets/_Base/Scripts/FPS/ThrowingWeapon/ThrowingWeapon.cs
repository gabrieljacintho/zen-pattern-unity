using FireRingStudio.Extensions;

namespace FireRingStudio.FPS.ThrowingWeapon
{
    public class ThrowingWeapon : UseEquipment.UseEquipment
    {
        private ThrowingWeaponFire _throwingWeaponFire;
        private ThrowingWeaponData _throwingWeaponData;
        
        public new ThrowingWeaponData Data
        {
            get
            {
                if (_throwingWeaponData == null)
                {
                    _throwingWeaponData = base.Data as ThrowingWeaponData;
                }
                
                return _throwingWeaponData;
            }
        }
        private ThrowingWeaponFire Fire
        {
            get
            {
                if (_throwingWeaponFire == null)
                {
                    _throwingWeaponFire = GetComponent<ThrowingWeaponFire>();
                }

                return _throwingWeaponFire;
            }
        }
        
        
        protected override void Setup()
        {
            base.Setup();
            gameObject.GetOrAddComponent<ThrowingWeaponFire>();
        }

        protected override void OnUse()
        {
            if (Fire == null || Data == null)
            {
                return;
            }
            
            CreateProjectile();
            Fire.ThrowProjectile(Data.ThrowParameters);
        }

        #region AnimationEvents
        
        private void CreateProjectile()
        {
            if (Fire == null || Data == null || Data.ProjectilePrefab == null)
            {
                return;
            }
            
            Fire.InstantiateProjectile(Data.ProjectilePrefab);
        }
        
        #endregion
    }
}