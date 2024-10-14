using FireRingStudio.FPS.Equipment;
using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentMovement : EquipmentMovement
    {
        private UseEquipment _useEquipment;
        
        protected override Equipment.Equipment Equipment => UseEquipment;
        private UseEquipment UseEquipment
        {
            get
            {
                if (_useEquipment == null)
                {
                    _useEquipment = GetComponent<UseEquipment>();
                }

                return _useEquipment;
            }
        }
        private new UseEquipmentData Data => UseEquipment.Data;


        protected override void Awake()
        {
            base.Awake();
            
            if (UseEquipment != null)
            {
                UseEquipment.OnStartUsingEvent.AddListener(OnStartUsing);
            }
        }

        protected override bool CanSprint()
        {
            bool value = base.CanSprint();

            if (Data == null)
            {
                return value;
            }
            
            if (UseEquipment.InUse)
            {
                value &= Data.UsingState.CanSprint;
            }
            
            return value;
        }

        private void OnStartUsing()
        {
            if (CurrentMovementState == MovementState.Sprinting)
            {
                if (Data != null && Data.UsingState.StopSprintingWhenEnter)
                {
                    PlayerMovement.StopSprinting();
                }
            }
        }
    }
}