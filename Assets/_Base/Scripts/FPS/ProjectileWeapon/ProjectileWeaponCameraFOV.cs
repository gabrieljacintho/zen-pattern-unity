using FireRingStudio.FPS.UseEquipment;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponCameraFOV : UseEquipmentCameraFOV
    {
        private ProjectileWeaponBase _projectileWeapon;
        private ProjectileWeaponAim _projectileWeaponAim;

        private bool _wasAiming;

        protected override Equipment.Equipment Equipment => ProjectileWeapon;
        private ProjectileWeaponBase ProjectileWeapon
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
        private new ProjectileWeaponData Data => ProjectileWeapon.Data;
        private bool IsAiming => _projectileWeaponAim != null && _projectileWeaponAim.IsAiming;


        protected override void Awake()
        {
            base.Awake();
            _projectileWeaponAim = GetComponent<ProjectileWeaponAim>();
        }

        protected override void UpdateCameraComponent(CameraFOVHandler cameraComponent)
        {
            if (IsAiming && Data != null && Data.ScopeData != null)
            {
                if (_projectileWeaponAim.CanShowScope)
                {
                    base.UpdateCameraComponent(cameraComponent);
                    cameraComponent.Snap();
                    _wasAiming = true;
                }
            }
            else
            {
                base.UpdateCameraComponent(cameraComponent);

                if (_wasAiming)
                {
                    cameraComponent.Snap();
                    _wasAiming = false;
                }
            }
        }
    }
}