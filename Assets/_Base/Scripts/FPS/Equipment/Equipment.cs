using System;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using FireRingStudio.Movement;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private EquipmentData _data;

        private PlayerMovement _playerMovement;

        private bool _loadingUserInterface;
        private float _userInterfaceTime;

        public EquipmentData Data => _data;
        public PlayerMovement PlayerMovement => _playerMovement;

        public bool IsEquipped { get; private set; }
        public virtual bool InUse { get; protected set; }
        public bool CanShowUserInterface { get; private set; }
        public Precision CurrentPrecision => GetCurrentPrecision();
        public MovementState CurrentMovementState => _playerMovement != null ? _playerMovement.CurrentState : MovementState.Idle;

        public Action PlayEquipFXs;
        public Action PlayUnequipFXs;
        
        [Space]
        [InspectorName("On Equip")] public UnityEvent OnEquipEvent;
        [InspectorName("On Unequip")] public UnityEvent OnUnequipEvent;


        protected virtual void Awake()
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
        }

        protected virtual void OnEnable()
        {
            Equip();
        }

        protected virtual void Update()
        {
            if (GameManager.IsPaused || !IsEquipped)
            {
                return;
            }

            if (Data != null)
            {
                MathHelper.DelayedAction(() =>
                {
                    CanShowUserInterface = true;
                }, Data.UserInterfaceDelay, ref _userInterfaceTime, ref _loadingUserInterface);
            }
        }
        
        public virtual void Equip(bool playFXs = true)
        {
            if (IsEquipped || !CanEquip())
            {
                return;
            }
            
            IsEquipped = true;

            CanShowUserInterface = Data != null && Data.UserInterfaceDelay <= 0f;
            _loadingUserInterface = Data != null && Data.UserInterfaceDelay > 0f;
            _userInterfaceTime = 0f;
            
            gameObject.SetActive(true);
            StopAllCoroutines();

            OnEquip();
            
            OnEquipEvent?.Invoke();
            
            if (playFXs)
            {
                PlayEquipFXs?.Invoke();
            }
        }

        protected virtual void OnEquip() { }

        public virtual void Unequip(bool playFXs = false)
        {
            if (!IsEquipped)
            {
                return;
            }
            
            IsEquipped = false;
            CanShowUserInterface = false;

            OnUnequip();
            
            OnUnequipEvent?.Invoke();
            
            void DisableObject() => gameObject.SetActive(false);
            
            if (playFXs)
            {
                PlayUnequipFXs?.Invoke();
                this.DoAfterSeconds(DisableObject, 3f);
            }
            else
            {
                DisableObject();
            }
        }

        protected virtual void OnUnequip() { }
        
        public virtual bool CanEquip()
        {
            return true;
        }
        
        public virtual void ResetToDefault()
        {
            Unequip();
        }
        
        public EquipmentStateData GetCurrentStateData(MovementState movementState)
        {
            MovementStateValues<EquipmentStateData> stateDataset = GetCurrentStateDataset();
            EquipmentStateData stateData = stateDataset.GetValue(movementState);

            if (stateData == null && Data != null)
            {
                stateData = Data.DefaultStateDataset.GetValue(movementState);
            }
            
            return stateData;
        }
        
        public virtual MovementStateValues<EquipmentStateData> GetCurrentStateDataset()
        {
            return Data != null ? Data.DefaultStateDataset : default;
        }
        
        public virtual EquipmentStateData GetCurrentStateData()
        {
            return GetCurrentStateData(CurrentMovementState);
        }

        protected virtual Precision GetCurrentPrecision()
        {
            MovementStateValues<Precision> stateDataset = Data != null ? Data.DefaultPrecisions : default;
            return stateDataset.GetValue(CurrentMovementState);
        }

        protected virtual void Reset()
        {
            Setup();
        }

        [ContextMenu("Setup")]
        protected virtual void Setup()
        {
            gameObject.GetOrAddComponent<EquipmentAnimator>();
            gameObject.GetOrAddComponent<EquipmentAnimation>();
            gameObject.GetOrAddComponent<EquipmentAudio>();
            gameObject.GetOrAddComponent<EquipmentArms>();
            gameObject.GetOrAddComponent<EquipmentMovement>();
            gameObject.GetOrAddComponent<EquipmentLook>();
            gameObject.GetOrAddComponent<EquipmentCameraFOV>();
            gameObject.GetOrAddComponent<EquipmentHeadBobbing>();
        }
    }
}