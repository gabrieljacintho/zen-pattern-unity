using System;
using FireRingStudio.Movement;
using Sirenix.OdinInspector;

namespace FireRingStudio.FPS.UseEquipment
{
    [Serializable]
    public struct EquipmentUseStateInfo
    {
        public MovementStateValues<EquipmentStateData> DataByMovementState;
        public bool AllowedWhileAirborne;
        public bool CanSprint;
        [ShowIf("CanSprint")]
        public bool StopSprintingWhenEnter;
    }
}