using System;
using FireRingStudio.Extensions;
using FireRingStudio.Interaction;
using FireRingStudio.Inventory;
using FireRingStudio.Movement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [DisallowMultipleComponent]
    public class ProjectileWeaponReload : ProjectileWeaponComponent
    {
        [SerializeField] private InputActionReference _reloadInput;

        private IInteractor[] _interactors;
        private PlayerMovement _playerMovement;

        private ItemPack AmmoClip => ProjectileWeapon.AmmoClip;
        private int AmmoPackSize
        {
            get => ProjectileWeapon.QuantityOfAmmoInInventory;
            set => ProjectileWeapon.QuantityOfAmmoInInventory = value;
        }
        
        public bool IsReloading { get; private set; }

        public event Action StartReloadAction;
        public event Action ReloadAction;
        public event Action EmptyReloadAction;
        public event Action StopReloadAction;
        public event Action EndReloadAction;
        
        [Space]
        public UnityEvent OnStartReload;
        public UnityEvent OnAddAmmo;


        protected override void Awake()
        {
            base.Awake();
            _interactors = GameObjectID.FindGameObjectWithID(GameObjectID.PlayerID, true)
                .GetInterfacesInChildren<IInteractor>(true);
            _playerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (_reloadInput != null)
            {
                _reloadInput.action.performed += StartReload;
            }
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            TryAutoReload();
        }

        private void OnDisable()
        {
            if (_reloadInput != null)
            {
                _reloadInput.action.performed -= StartReload;
            }
            
            StopReload();
        }
        
        public void StartReload()
        {
            if (IsReloading || AmmoClip == null || AmmoClip.IsFull || AmmoPackSize == 0)
            {
                return;
            }
            
            MovementState movementState = _playerMovement != null ? _playerMovement.CurrentState : MovementState.Idle;
            
            if (movementState == MovementState.Airborne)
            {
                if (!Data.ReloadingState.AllowedWhileAirborne)
                {
                    return;
                }
            }

            ProjectileWeapon.StopUsing();

            if (ProjectileWeapon.Aim != null)
            {
                ProjectileWeapon.Aim.StopAim();
            }

            if (AmmoClip.IsEmpty && Data.HasEmptyReload)
            {
                EmptyReloadAction?.Invoke();
            }
            else if (Data.ProgressiveReload)
            {
                StartReloadAction?.Invoke();
            }
            else
            {
                ReloadAction?.Invoke();
            }

            IsReloading = true;
            
            OnStartReload?.Invoke();
        }

        private void StartReload(InputAction.CallbackContext context)
        {
            if (!GameManager.InGame)
            {
                return;
            }
            
            if (Data == null || !Data.CanReloadWhileInteracting)
            {
                foreach (IInteractor interactor in _interactors)
                {
                    if (interactor.IsInteracting || interactor.CanInteract())
                    {
                        return;
                    }
                }
            }
            
            StartReload();
        }

        public void StopReload()
        {
            StopReloadAction?.Invoke();
            IsReloading = false;
        }
        
        private void TryAutoReload()
        {
            if (Data == null || !Data.CanAutoReload || AmmoClip == null || !AmmoClip.IsEmpty)
            {
                return;
            }

            if (Equipment.InUse)
            {
                return;
            }
            
            StartReload();
        }
        
        private void AddAmmo(int amount)
        {
            if (AmmoClip == null)
            {
                return;
            }
            
            amount = Mathf.Min(amount, AmmoPackSize);
            amount = Mathf.Clamp(amount, 0, AmmoClip.MaxSize - AmmoClip.Size);
            if (amount == 0)
            {
                return;
            }
            
            AmmoClip.Size += amount;
            AmmoPackSize -= amount;
            
            OnAddAmmo?.Invoke();
        }

        #region AnimationEvents
        
        private void EmptyReload()
        {
            if (Data == null)
            {
                return;
            }

            if (Data.ProgressiveEmptyReload)
            {
                AddAmmo(1);
            }
            else if (AmmoClip != null)
            {
                AddAmmo(AmmoClip.MaxSize);
            }
        }

        private void EndEmptyReload()
        {
            if (Data != null && Data.ProgressiveEmptyReload)
            {
                if (AmmoClip != null && !AmmoClip.IsFull && AmmoPackSize > 0)
                {
                    StartReloadAction?.Invoke();
                    return;
                }

                EndReloadAction?.Invoke();
            }
            
            IsReloading = false;
        }

        private void EndStartReload()
        {
            ReloadAction?.Invoke();
        }

        private new void Reload()
        {
            if (Data == null)
            {
                return;
            }

            if (Data.ProgressiveReload)
            {
                AddAmmo(1);
            }
            else if (AmmoClip != null)
            {
                AddAmmo(AmmoClip.MaxSize);
            }
        }

        private void EndReload()
        {
            if (Data != null && Data.ProgressiveReload)
            {
                if (AmmoClip != null && !AmmoClip.IsFull && AmmoPackSize > 0)
                {
                    ReloadAction?.Invoke();
                    return;
                }

                EndReloadAction?.Invoke();
            }

            IsReloading = false;
        }
        
        #endregion
    }
}