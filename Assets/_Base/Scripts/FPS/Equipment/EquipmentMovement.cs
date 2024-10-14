using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentMovement : EquipmentComponent
    {
        protected override bool CanUpdate => base.CanUpdate && PlayerMovement != null;
        protected PlayerMovement PlayerMovement => Equipment.PlayerMovement;
        protected MovementState CurrentMovementState => Equipment.CurrentMovementState;
        
        
        protected virtual void LateUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }
            
            PlayerMovement.SpeedScale.SetComponentValue(this, GetMoveSpeedScale());
            PlayerMovement.CanSprint.SetComponentValue(this, CanSprint());
        }
        
        protected override void OnUnequip()
        {
            base.OnUnequip();
            
            if (PlayerMovement != null)
            {
                PlayerMovement.SpeedScale.RemoveComponent(this);
                PlayerMovement.CanSprint.RemoveComponent(this);
            }
        }
        
        protected virtual float GetMoveSpeedScale()
        {
            return Data != null ? Data.MoveSpeedScale : 1f;
        }
        
        protected virtual bool CanSprint()
        {
            return Data == null || Data.CanSprint;
        }
    }
}