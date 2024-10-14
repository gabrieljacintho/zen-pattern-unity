using System;
using FireRingStudio.Cache;

namespace FireRingStudio.FPS
{
    [Serializable]
    public struct AnimationInfo
    {
        public string TriggerName;
        public string SpeedParameterName;
        public float Speed;
        
        public int TriggerId => !string.IsNullOrEmpty(TriggerName) ? AnimationParameterIds.GetId(TriggerName) : -1;
        public int SpeedParameterId => !string.IsNullOrEmpty(SpeedParameterName) ? AnimationParameterIds.GetId(SpeedParameterName) : -1;
    }
}