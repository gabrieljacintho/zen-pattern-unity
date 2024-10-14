using System;
using FireRingStudio.Cache;
using FireRingStudio.FPS.ThrowingWeapon;
using FireRingStudio.FPS.UseEquipment;
using FireRingStudio.Inventory;
using FireRingStudio.Movement;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [Serializable]
    public enum FireMode
    {
        SemiAutomatic,
        Automatic,
        Burst
    }
    
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Projectile Weapon Data", fileName = "New Projectile Weapon Data")]
    public class ProjectileWeaponData : UseEquipmentData
    {
        #region Fire
        
        [FoldoutGroup("Fire")] public FireMode FireMode;
        [FoldoutGroup("Fire")] public ItemPack AmmoClip;
        [FoldoutGroup("Fire"), Range(0f, 1f)] public float InitialAmmoClipFillAmount = 1f;
        [FoldoutGroup("Fire"), Min(1)] public int ProjectilesPerShot = 1;
        [FoldoutGroup("Fire")] public FloatReference MaxDistanceReference = new(300f);
        [FoldoutGroup("Fire")] public ThrowParameters ThrowParameters;
        
        public ProjectileData ProjectileData => AmmoClip?.Data as ProjectileData;
        public float MaxDistance => MaxDistanceReference?.Value ?? 0f;
                
        [Header("VFXs")]
        [FoldoutGroup("Fire")] public GameObject MuzzleFlashPrefab;
        [FoldoutGroup("Fire")] public GameObject TracerPrefab;

        [Header("Casing Ejection")]
        [FoldoutGroup("Fire")] public bool DropCasing = true;
        [ShowIf("DropCasing")]
        [FoldoutGroup("Fire")] public float CasingScale = 1f;
        [ShowIf("DropCasing")]
        [FoldoutGroup("Fire")] public float CasingSpin;
        [ShowIf("DropCasing")]
        [FoldoutGroup("Fire")] public Vector3 CasingVelocity;
        
        [Header("SFXs")]
        [FoldoutGroup("Fire")] public EventReference HandlingSFX;
        [FoldoutGroup("Fire")] public EventReference DryFireSFX;
        
        #endregion
        
        #region Damage
        
        [FoldoutGroup("Damage")] public DamageParameters DamageParameters;
        [FoldoutGroup("Damage")] public AnimationCurve DamageCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        
        #endregion
        
        #region Recoil
        
        [FoldoutGroup("Recoil")] public Vector3 RecoilForce;
        [FoldoutGroup("Recoil")] public float RecoilSnappiness;
        [FoldoutGroup("Recoil")] public float RecoilReturnSpeed;
        
        [Header("Precision")]
        [FoldoutGroup("Recoil")] public float PrecisionRadiusRecoil;
        [FoldoutGroup("Recoil")] public float PrecisionSnappiness;
        [FoldoutGroup("Recoil")] public float PrecisionReturnSpeed;
        
        #endregion
        
        #region Push
        
        [FoldoutGroup("Push")] public float PushForce = 10f;
        [FoldoutGroup("Push")] public LayerMask PushLayerMask;
        
        #endregion
        
        #region Reload

        [FoldoutGroup("Reload")] public EquipmentUseStateInfo ReloadingState;
        [FoldoutGroup("Reload")] public bool ProgressiveReload;
        [FoldoutGroup("Reload")] public bool HasEmptyReload = true;
        [ShowIf("HasEmptyReload")]
        [FoldoutGroup("Reload")] public bool ProgressiveEmptyReload;
        [FoldoutGroup("Reload")] public bool CanAutoReload = true;
        [FoldoutGroup("Reload")] public bool CanInterruptReloading;
        [FoldoutGroup("Reload")] public bool CanReloadWhileInteracting;

        [Header("Animations")]
        [FoldoutGroup("Reload")] public bool HasReloadAnimation = true;
        [ShowIf("HasReloadAnimation")]
        [FoldoutGroup("Reload")] public AnimationInfo StartReloadAnimationInfo;
        [ShowIf("HasReloadAnimation")]
        [FoldoutGroup("Reload")] public AnimationInfo ReloadAnimationInfo;
        [ShowIf("@HasReloadAnimation && HasEmptyReload")]
        [FoldoutGroup("Reload")] public AnimationInfo EmptyReloadAnimationInfo;
        [ShowIf("HasReloadAnimation")]
        [FoldoutGroup("Reload")] public AnimationInfo EndReloadAnimationInfo;
        [ShowIf("HasReloadAnimation")]
        [FoldoutGroup("Reload")] public string IdleAnimationIndexParameterName = "Idle Index";
        [ShowIf("HasReloadAnimation")]
        [FoldoutGroup("Reload")] public string FireAnimationIndexParameterName = "Fire Index";

        public int IdleAnimationIndexParameterId => !string.IsNullOrEmpty(IdleAnimationIndexParameterName)
            ? AnimationParameterIds.GetId(IdleAnimationIndexParameterName) : -1;
        public int FireAnimationIndexParameterId => !string.IsNullOrEmpty(FireAnimationIndexParameterName)
            ? AnimationParameterIds.GetId(FireAnimationIndexParameterName) : -1;
        
        [Header("VFXs")]
        [FoldoutGroup("Reload")] public bool DropEmptyMagazine = true;
        [ShowIf("DropEmptyMagazine")]
        [FoldoutGroup("Reload")] public GameObject MagazinePrefab;
        [ShowIf("DropEmptyMagazine")]
        [FoldoutGroup("Reload")] public Vector3 MagazineVelocity;
        [ShowIf("DropEmptyMagazine")]
        [FoldoutGroup("Reload")] public Vector3 MagazineAngularVelocity;
        
        [Header("SFXs")]
        [FoldoutGroup("Reload")] public EventReference StartReloadSFX;
        [FoldoutGroup("Reload")] public EventReference ReloadSFX;
        [ShowIf("HasEmptyReload")]
        [FoldoutGroup("Reload")] public EventReference EmptyReloadSFX;
        [FoldoutGroup("Reload")] public EventReference EndReloadSFX;
        
        #endregion
        
        #region Aim

        [FoldoutGroup("Aim")] public EquipmentUseStateInfo AimingState;
        [FoldoutGroup("Aim")] public MovementStateValues<Precision> AimingPrecisions;
        [FoldoutGroup("Aim"), Range(0f, 1f)] public float AimSensitivityScale = 0.6f;
        [FoldoutGroup("Aim"), Range(0f, 1f)] public float GamepadAimSensitivityScale = 0.3f;
        
        [Header("SFXs")]
        [FoldoutGroup("Aim")] public EventReference AimSFX;

        #endregion

        #region Attachments

        [FoldoutGroup("Attachments")] public ItemData ScopeData;
        [FoldoutGroup("Attachments"), ShowIf("@ScopeData != null")] public float ScopeDelay = 0.1f;

        #endregion
    }
}