using System;
using FireRingStudio.Extensions;
using FireRingStudio.FPS.Equipment;
using FireRingStudio.Helpers;
using FireRingStudio.Inventory;
using FireRingStudio.Movement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.FPS.UseEquipment
{
    public class UseEquipment : Equipment.Equipment
    {
        [Header("Use")]
        [SerializeField] protected InventoryData _inventory;
        
        [Header("Input")]
        [SerializeField] protected InputActionReference _useInput;
        
        [Header("Settings")]
        [SerializeField] private bool _startPrepared = true;
        [SerializeField] private bool _canHold;
        [SerializeField] private bool _canUseWhenRelease = true;
        [SerializeField] private bool _endUsingWhenUse = true;

        [Space]
        [SerializeField] protected bool _consumeItemOnUse;
        [SerializeField] protected bool _toggleEquipmentOnUse;
        [SerializeField] protected bool _useOnEquip;

        private UseEquipmentData _useEquipmentData;
        private InventorySlotSelector _inventorySlotSelector;

        protected float _lastUseTime;
        
        private bool _inUse;
        private bool _holding;
        private bool _prepared;

        private bool _loadingUseUserInterface;
        private float _useUserInterfaceTime;

        public new UseEquipmentData Data
        {
            get
            {
                if (_useEquipmentData == null)
                {
                    _useEquipmentData = base.Data as UseEquipmentData;
                }
                
                return _useEquipmentData;
            }
        }

        public override bool InUse
        {
            get => _inUse || Time.time - _lastUseTime < 0.1f;
            protected set => _inUse = value;
        }
        public bool CanShowUseUserInterface { get; private set; }
        protected bool CanUpdate => !GameManager.IsPaused && IsEquipped;

        public event Action StopUseAction;
        public event Action HoldAction;
        public event Action ReleaseAction;
        public event Action EndUseAction;
        
        [Space]
        [InspectorName("On Start Using")] public UnityEvent OnStartUsingEvent;
        [InspectorName("On Use")] public UnityEvent OnUseEvent;


        protected override void Awake()
        {
            base.Awake();
            _inventorySlotSelector = GetComponentInParent<InventorySlotSelector>();
        }

        protected override void Update()
        {
            base.Update();

            if (!CanUpdate || !GameManager.InGame)
            {
                return;
            }

            if (Data != null)
            {
                MathHelper.DelayedAction(() =>
                {
                    CanShowUseUserInterface = true;
                }, Data.UseUserInterfaceDelay, ref _useUserInterfaceTime, ref _loadingUseUserInterface);
            }

            if (_useInput != null)
            {
                OnUpdateUseInput(_useInput.action);

                if (_useInput.action.WasReleasedThisFrame())
                {
                    OnUseInputWasReleasedThisFrame();
                }
            }
        }

        protected virtual void OnDisable()
        {
            StopUsing();
        }

        public override void Equip(bool playFXs = true)
        {
            if (IsEquipped)
            {
                return;
            }

            base.Equip(!_useOnEquip && playFXs);

            CanShowUseUserInterface = false;
            _loadingUseUserInterface = false;
            _useUserInterfaceTime = 0f;

            if (_useOnEquip)
            {
                StartUsing();
            }
        }

        protected override void OnUnequip()
        {
            CanShowUseUserInterface = false;
            _loadingUseUserInterface = false;
            _useUserInterfaceTime = 0f;
        }

        public override bool CanEquip()
        {
            return base.CanEquip() && (!_useOnEquip || CanUse());
        }

        public override MovementStateValues<EquipmentStateData> GetCurrentStateDataset()
        {
            if (InUse && Data != null)
            {
                return Data.UsingState.DataByMovementState;
            }
            
            return base.GetCurrentStateDataset();
        }

        protected override void Setup()
        {
            gameObject.GetOrAddComponent<EquipmentAnimator>();
            gameObject.GetOrAddComponent<UseEquipmentAnimation>();
            gameObject.GetOrAddComponent<UseEquipmentAudio>();
            gameObject.GetOrAddComponent<EquipmentArms>();
            gameObject.GetOrAddComponent<UseEquipmentMovement>();
            gameObject.GetOrAddComponent<UseEquipmentLook>();
            gameObject.GetOrAddComponent<UseEquipmentCameraFOV>();
            gameObject.GetOrAddComponent<EquipmentHeadBobbing>();
        }

        protected virtual void OnUpdateUseInput(InputAction useInput)
        {
            if (useInput.WasPressedThisFrame())
            {
                StartUsing();
            }
        }

        protected virtual void OnUseInputWasReleasedThisFrame()
        {
            if (_holding)
            {
                Release();
            }
        }
        
        protected virtual void StartUsing()
        {
            if (InUse || !CanUse())
            {
                return;
            }
            
            InUse = true;
            
            _prepared = _startPrepared;
            _holding = _canHold;

            CanShowUseUserInterface = Data != null && Data.UseUserInterfaceDelay <= 0f;
            _loadingUseUserInterface = Data != null && Data.UseUserInterfaceDelay > 0f;
            _useUserInterfaceTime = 0f;

            OnStartUsing();
            
            _lastUseTime = Time.time;
            
            OnStartUsingEvent?.Invoke();
            
            if (_prepared && !_holding)
            {
                Release();
            }
        }

        protected virtual void OnStartUsing() { }

        public virtual void StopUsing()
        {
            InUse = false;
            _holding = false;
            _prepared = false;

            CanShowUseUserInterface = false;
            _loadingUseUserInterface = false;
            _useUserInterfaceTime = 0f;

            if (_useInput != null)
            {
                _useInput.action.Reset();
            }
            
            StopUseAction?.Invoke();
        }

        private void Hold()
        {
            HoldAction?.Invoke();
        }
        
        private void Release()
        {
            if (!InUse)
            {
                return;
            }
            
            _holding = false;
            
            ReleaseAction?.Invoke();
            
            if (_canUseWhenRelease && _prepared)
            {
                Use();
            }
        }
        
        protected virtual bool CanUse()
        {
            if (Data == null)
            {
                return false;
            }

            if (CurrentMovementState == MovementState.Airborne && !Data.UsingState.AllowedWhileAirborne)
            {
                return false;
            }
            
            return Time.time >= _lastUseTime + 60f / Data.UsesPerMinute;
        }
        
        #region AnimationEvents

        private void Prepared()
        {
            if (!InUse)
            {
                return;
            }
            
            _prepared = true;

            if (_holding)
            {
                Hold();
            }
            else
            {
                Release();
            }
        }
        
        private void Use()
        {
            if (!InUse)
            {
                return;
            }
            
            OnUse();
            
            OnUseEvent?.Invoke();
            
            if (_endUsingWhenUse)
            {
                EndUsing();
            }
        }

        protected virtual void OnUse() { }

        private void EndUsing()
        {
            if (!InUse)
            {
                return;
            }
            
            InUse = false;
            _holding = false;
            _prepared = false;

            CanShowUseUserInterface = false;
            _loadingUseUserInterface = false;
            _useUserInterfaceTime = 0f;

            EndUseAction?.Invoke();

            if (_toggleEquipmentOnUse && _inventorySlotSelector != null)
            {
                _inventorySlotSelector.TryToggleSlot();
            }

            if (_consumeItemOnUse && _inventory != null && Data != null)
            {
                _inventory.RemoveItem(Data);
            }

            OnEndUsing();
        }

        protected virtual void OnEndUsing() { }
        
        #endregion
    }
}