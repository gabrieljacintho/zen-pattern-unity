using FireRingStudio.FPS.UseEquipment;
using FireRingStudio.Input;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponLook : UseEquipmentLook
    {
        private ProjectileWeaponBase _projectileWeapon;
        private ProjectileWeaponAim _projectileWeaponAim;

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
        
        protected override float GetLookSensitivityScale()
        {
            float value = base.GetLookSensitivityScale();

            if (Data == null)
            {
                return value;
            }
            
            if (IsAiming)
            {
                if (InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse)
                {
                    value *= Data.AimSensitivityScale;
                }
                else
                {
                    value *= Data.GamepadAimSensitivityScale;
                }
            }
            
            return value;
        }
    }
}