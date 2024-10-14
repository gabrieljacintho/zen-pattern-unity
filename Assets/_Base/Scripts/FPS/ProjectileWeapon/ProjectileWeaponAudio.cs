using FireRingStudio.Extensions;
using FireRingStudio.FPS.UseEquipment;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(ProjectileWeaponBase))]
    public class ProjectileWeaponAudio : UseEquipmentAudio
    {
        private ProjectileWeaponBase _projectileWeapon;
        
        private StudioEventEmitter _handlingAudioSource;
        
        private StudioEventEmitter _reloadAudioSource;
        private StudioEventEmitter _startReloadingAudioSource;
        private StudioEventEmitter _endReloadingStateAudioSource;
        private StudioEventEmitter _emptyReloadAudioSource;
        
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
        private ProjectileWeaponReload Reload => Equipment != null ? ProjectileWeapon.Reload : null;
        private ProjectileWeaponAim Aim => Equipment != null ? ProjectileWeapon.Aim : null;

        
        protected override void Awake()
        {
            base.Awake();
            
            ProjectileWeapon.OnUseEvent.AddListener(PlayHandlingSFX);
            ProjectileWeapon.OnDryFire.AddListener(PlayDryFireSFX);
            ProjectileWeapon.StopUseAction += StopHandlingSFX;

            if (Reload != null)
            {
                Reload.StartReloadAction += PlayStartReloadingSFX;
                Reload.ReloadAction += PlayReloadSFX;
                Reload.EmptyReloadAction += PlayEmptyReloadSFX;
                Reload.StopReloadAction += StopReloadSFXs;
                Reload.EndReloadAction += PlayEndReloadingStateSFX;
            }

            if (Aim != null)
            {
                Aim.OnStartAim.AddListener(PlayAimSFX);
            }
        }

        #region Fire
        
        private void PlayHandlingSFX()
        {
            if (Data != null && !Data.HandlingSFX.IsNull)
            {
                _handlingAudioSource = PlaySFX(Data.HandlingSFX);
            }
        }
        
        private void StopHandlingSFX()
        {
            if (_handlingAudioSource != null)
            {
                _handlingAudioSource.Stop();
            }
        }
        
        private void PlayDryFireSFX()
        {
            if (Data != null && !Data.DryFireSFX.IsNull)
            {
                Data.DryFireSFX.Play(transform.position, false);
            }
        }
        
        #endregion
        
        #region Reload

        private void PlayStartReloadingSFX()
        {
            if (Data != null && !Data.StartReloadSFX.IsNull)
            {
                _startReloadingAudioSource = PlaySFX(Data.StartReloadSFX);
            }
        }
        
        private void PlayEndReloadingStateSFX()
        {
            if (Data != null && !Data.EndReloadSFX.IsNull)
            {
                _endReloadingStateAudioSource = PlaySFX(Data.EndReloadSFX);
            }
        }

        private void PlayReloadSFX()
        {
            if (Data != null && !Data.ReloadSFX.IsNull)
            {
                _reloadAudioSource = PlaySFX(Data.ReloadSFX);
            }
        }

        private void PlayEmptyReloadSFX()
        {
            if (Data != null && !Data.EmptyReloadSFX.IsNull)
            {
                _emptyReloadAudioSource = PlaySFX(Data.EmptyReloadSFX);
            }
        }
        
        private void StopReloadSFXs()
        {
            if (_startReloadingAudioSource != null)
            {
                _startReloadingAudioSource.Stop();
            }
            
            if (_reloadAudioSource != null)
            {
                _reloadAudioSource.Stop();
            }
            
            if (_emptyReloadAudioSource != null)
            {
                _emptyReloadAudioSource.Stop();
            }

            if (_endReloadingStateAudioSource != null)
            {
                _endReloadingStateAudioSource.Stop();
            }
        }
        
        #endregion
        
        #region Aim

        private void PlayAimSFX()
        {
            if (Data != null && !Data.AimSFX.IsNull)
            {
                PlaySFX(Data.AimSFX);
            }
        }
        
        #endregion
    }
}