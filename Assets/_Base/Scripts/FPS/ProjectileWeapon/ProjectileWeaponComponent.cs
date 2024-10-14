using FireRingStudio.FPS.UseEquipment;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponComponent : UseEquipmentComponent
    {
        private ProjectileWeaponBase _projectileWeapon;

        protected override Equipment.Equipment Equipment => ProjectileWeapon;
        protected ProjectileWeaponBase ProjectileWeapon
        {
            get
            {
                if (_projectileWeapon == null)
                {
                    _projectileWeapon = GetComponent<ProjectileWeaponBase>();
                }

                return _projectileWeapon;
            }
        }
        protected new ProjectileWeaponData Data => ProjectileWeapon.Data;
        protected ProjectileWeaponReload Reload => ProjectileWeapon != null ? ProjectileWeapon.Reload : null;
        protected ProjectileWeaponAim Aim => ProjectileWeapon != null ? ProjectileWeapon.Aim : null;


        protected override void Awake()
        {
            base.Awake();
            ProjectileWeapon.OnDryFire.AddListener(OnDryFire);
        }
        
        protected virtual void OnDryFire() { }
    }
}