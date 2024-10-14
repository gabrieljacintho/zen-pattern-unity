using FireRingStudio.FPS.UseEquipment;
using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponMovement : UseEquipmentMovement
    {
        private ProjectileWeaponBase _projectileWeapon;

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
        private bool IsReloading => ProjectileWeapon != null && ProjectileWeapon.IsReloading;
        private bool IsAiming => ProjectileWeapon != null && ProjectileWeapon.IsAiming;
        
        
        protected override void Awake()
        {
            base.Awake();

            if (ProjectileWeapon.Reload != null)
            {
                ProjectileWeapon.Reload.OnStartReload.AddListener(OnStartReload);
            }

            if (ProjectileWeapon.Aim != null)
            {
                ProjectileWeapon.Aim.OnStartAim.AddListener(OnStartAim);
            }
        }

        protected override bool CanSprint()
        {
            bool value = base.CanSprint();
            
            if (Data == null)
            {
                return value;
            }

            if (IsReloading)
            {
                value &= Data.ReloadingState.CanSprint;
            }

            if (IsAiming)
            {
                value &= Data.AimingState.CanSprint;
            }
            
            return value;
        }
        
        private void OnStartReload()
        {
            if (CurrentMovementState == MovementState.Sprinting)
            {
                if (Data != null && Data.ReloadingState.StopSprintingWhenEnter)
                {
                    PlayerMovement.StopSprinting();
                }
            }
        }
        
        private void OnStartAim()
        {
            if (CurrentMovementState == MovementState.Sprinting)
            {
                if (Data != null && Data.AimingState.StopSprintingWhenEnter)
                {
                    PlayerMovement.StopSprinting();
                }
            }
        }
    }
}