using FireRingStudio.FPS.UseEquipment;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponAnimation : UseEquipmentAnimation
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
        private ProjectileWeaponReload Reload => ProjectileWeapon != null ? ProjectileWeapon.Reload : null;
        private ProjectileWeaponAim Aim => ProjectileWeapon != null ? ProjectileWeapon.Aim : null;


        private void OnDisable()
        {
            StopReloadAnimation();
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            if (Data == null || !Data.HasReloadAnimation)
            {
                return;
            }

            UpdateAnimationSpeed(Data.StartReloadAnimationInfo);
            UpdateAnimationSpeed(Data.ReloadAnimationInfo);
            UpdateAnimationSpeed(Data.EndReloadAnimationInfo);

            if (Data.HasEmptyReload)
            {
                UpdateAnimationSpeed(Data.EmptyReloadAnimationInfo);
            }
        }

        protected override void OnInitializeFirst()
        {
            base.OnInitializeFirst();
            
            if (Reload != null)
            {
                Reload.StartReloadAction += PlayStartReloadAnimation;
                Reload.ReloadAction += PlayReloadAnimation;
                Reload.EmptyReloadAction += PlayEmptyReloadAnimation;
                Reload.StopReloadAction += StopReloadAnimation;
                Reload.EndReloadAction += PlayEndReloadAnimation;
            }
            
            if (Aim != null)
            {
                Aim.OnStartAim.AddListener(EnableAimAnimation);
                Aim.OnStopAim.AddListener(DisableAimAnimation);
            }
        }

        #region Reload

        private void PlayStartReloadAnimation()
        {
            if (Data != null)
            {
                SetTrigger(Data.StartReloadAnimationInfo);
            }
        }
        
        private void PlayReloadAnimation()
        {
            if (Data != null)
            {
                SetTrigger(Data.ReloadAnimationInfo);
            }
        }

        private void PlayEmptyReloadAnimation()
        {
            if (Data != null)
            {
                SetTrigger(Data.EmptyReloadAnimationInfo);
            }
        }
        
        private void PlayEndReloadAnimation()
        {
            if (Data != null)
            {
                SetTrigger(Data.EndReloadAnimationInfo);
            }
        }
        
        private void StopReloadAnimation()
        {
            if (Data == null)
            {
                return;
            }
            
            ResetTrigger(Data.StartReloadAnimationInfo);
            ResetTrigger(Data.ReloadAnimationInfo);
            ResetTrigger(Data.EmptyReloadAnimationInfo);
            ResetTrigger(Data.EndReloadAnimationInfo);
        }
        
        #endregion
        
        #region Aim
        
        private void SetAimAnimation(bool enable)
        {
            if (Data == null)
            {
                return;
            }

            int parameterId = Data.IdleAnimationIndexParameterId;
            if (parameterId != -1)
            {
                EquipmentAnimator.SetInteger(parameterId, enable ? 0 : 1);
            }

            parameterId = Data.FireAnimationIndexParameterId;
            if (parameterId != -1)
            {
                EquipmentAnimator.SetFloat(parameterId, enable ? 1 : 0);
            }
        }

        private void EnableAimAnimation()
        {
            SetAimAnimation(true);
        }
        
        private void DisableAimAnimation()
        {
            SetAimAnimation(false);
        }
        
        #endregion
    }
}