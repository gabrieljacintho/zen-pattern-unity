using FireRingStudio.Extensions;
using FireRingStudio.FPS.Equipment;
using FireRingStudio.FPS.UseEquipment;
using FireRingStudio.Inventory;
using FireRingStudio.Movement;
using FireRingStudio.Save;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    public abstract class ProjectileWeaponBase : UseEquipment.UseEquipment, ISave
    {
        private ProjectileWeaponData _projectileWeaponData;

        private int _pendingShots;

        public new ProjectileWeaponData Data
        {
            get
            {
                if (_projectileWeaponData == null)
                {
                    _projectileWeaponData = base.Data as ProjectileWeaponData;
                }

                return _projectileWeaponData;
            }
        }

        public override bool InUse => base.InUse;

        public ItemPack AmmoClip => Data != null ? Data.AmmoClip : null;
        public ProjectileWeaponReload Reload { get; private set; }
        public ProjectileWeaponAim Aim { get; private set; }
        
        public bool IsReloading => Reload != null && Reload.IsReloading;
        public bool IsAiming => Aim != null && Aim.IsAiming;
        public int QuantityOfAmmoInInventory
        {
            get => GetQuantityOfAmmoInInventory();
            set => SetQuantityOfAmmoInInventory(value);
        }

        public string SaveKey => Data.Id;

        public UnityEvent OnDryFire;
        
        
        protected override void Awake()
        {
            base.Awake();
            Reload = GetComponent<ProjectileWeaponReload>();
            Aim = GetComponent<ProjectileWeaponAim>();
            SaveManager.RegisterSave(this);
        }

        protected virtual void FixedUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }

            if (_pendingShots > 0)
            {
                StartUsing();
            }
        }

        private void OnDestroy()
        {
            SaveManager.UnregisterSave(this);
        }

        protected override void Setup()
        {
            gameObject.GetOrAddComponent<ProjectileWeaponReload>();
            gameObject.GetOrAddComponent<ProjectileWeaponAim>();
            gameObject.GetOrAddComponent<EquipmentAnimator>();
            gameObject.GetOrAddComponent<ProjectileWeaponAnimation>();
            gameObject.GetOrAddComponent<ProjectileWeaponVFX>();
            gameObject.GetOrAddComponent<ProjectileWeaponAudio>();
            gameObject.GetOrAddComponent<UseEquipmentArms>();
            gameObject.GetOrAddComponent<ProjectileWeaponMovement>();
            gameObject.GetOrAddComponent<ProjectileWeaponLook>();
            gameObject.GetOrAddComponent<ProjectileWeaponRecoil>();
            gameObject.GetOrAddComponent<ProjectileWeaponCameraFOV>();
            gameObject.GetOrAddComponent<EquipmentHeadBobbing>();
        }
        
        public override void ResetToDefault()
        {
            base.ResetToDefault();
            Load();
        }
        
        public override MovementStateValues<EquipmentStateData> GetCurrentStateDataset()
        {
            if (Data == null)
            {
                return base.GetCurrentStateDataset();
            }

            if (IsReloading)
            {
                return Data.ReloadingState.DataByMovementState;
            }
            
            if (IsAiming)
            {
                return Data.AimingState.DataByMovementState;
            }

            return base.GetCurrentStateDataset();
        }
        
        protected override Precision GetCurrentPrecision()
        {
            return Aim != null ? Aim.Precision : base.GetCurrentPrecision();
        }

        protected override void OnUpdateUseInput(InputAction useInput)
        {
            if (!CanUse())
            {
                return;
            }

            if (Data.FireMode == FireMode.Burst && _pendingShots != 0)
            {
                return;
            }
            
            if (useInput.WasPressedThisFrame())
            {
                AddPendingShots();
            }
            else if (Data.FireMode == FireMode.Automatic && useInput.IsPressed())
            {
                AddPendingShots();
            }
        }
        
        protected override void StartUsing()
        {
            if (!CanUse())
            {
                return;
            }
            
            if (IsReloading)
            {
                if (Data.CanInterruptReloading && AmmoClip != null && !AmmoClip.IsEmpty)
                {
                    Reload.StopReload();
                }
                else
                {
                    StopUsing();
                    return;
                }
            }

            if (AmmoClip == null || AmmoClip.IsEmpty)
            {
                DryFire();
                StopUsing();
                return;
            }
            
            base.StartUsing();
            
            _pendingShots--;
        }

        protected abstract void FireProjectile();

        protected override void OnUse()
        {
            IEnumerator Routine()
            {
                for (int i = 0; i < Data.ProjectilesPerShot; i++)
                {
                    FireProjectile();

                    if (Data.FireMode == FireMode.Burst)
                    {
                        break;
                    }
                    else if (i + 1 < Data.ProjectilesPerShot)
                    {
                        yield return null;
                    }
                }
            }

            if (UpdateManager.Instance != null)
            {
                UpdateManager.Instance.StartCoroutine(Routine());
            }

            AmmoClip.Size--;
        }

        public override void StopUsing()
        {
            base.StopUsing();
            _pendingShots = 0;
        }
        
        private void DryFire()
        {
            _lastUseTime = Time.time;
            OnDryFire?.Invoke();
        }
        
        private void AddPendingShots()
        {
            if (Data != null && Data.FireMode == FireMode.Burst)
            {
                _pendingShots = Data.ProjectilesPerShot;
            }
            else
            {
                _pendingShots = 1;
            }
        }
        
        private int GetQuantityOfAmmoInInventory()
        {
            if (_inventory == null || Data == null)
            {
                return 0;
            }

            ItemData ammoData = Data.AmmoClip?.Data;
            
            return ammoData != null ? _inventory.GetQuantityOfItem(ammoData) : 0;
        }
        
        private void SetQuantityOfAmmoInInventory(int value)
        {
            if (_inventory == null || Data == null)
            {
                return;
            }
            
            ItemData ammoData = Data.AmmoClip?.Data;
            if (ammoData == null)
            {
                return;
            }
            
            _inventory.SetQuantityOfItem(ammoData, value);
        }

        #region Save

        public void Save(ES3Settings settings)
        {
            ES3.Save(SaveKey, Data.AmmoClip.Size, settings);
        }

        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            int defaultValue = Mathf.RoundToInt(Data.AmmoClip.MaxSize * Data.InitialAmmoClipFillAmount);
            Data.AmmoClip.Size = ES3.Load(SaveKey, defaultValue, settings);
        }

        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(SaveKey, settings);
        }

        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));

        #endregion
    }
}