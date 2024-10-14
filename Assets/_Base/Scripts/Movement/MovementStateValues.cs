using System;

namespace FireRingStudio.Movement
{
    [Serializable]
    public struct MovementStateValues<T>
    {
        public T IdleValue;
        public T WalkingValue;
        public T SprintingValue;
        public T AirborneValue;

        
        public MovementStateValues(T idleValue, T walkingValue, T sprintingValue, T airborneValue)
        {
            IdleValue = idleValue;
            WalkingValue = walkingValue;
            SprintingValue = sprintingValue;
            AirborneValue = airborneValue;
        }

        public MovementStateValues(T defaultValue) : this(defaultValue, defaultValue, defaultValue, defaultValue)
        {
            
        }

        public T GetValue(MovementState movementState)
        {
            switch (movementState)
            {
                case MovementState.Idle:
                    return IdleValue;
                
                case MovementState.Walking:
                    return WalkingValue;
                
                case MovementState.Sprinting:
                    return SprintingValue;
                
                case MovementState.Airborne:
                    return AirborneValue;
                
                default:
                    return default;
            }
        }
    }
}