using BehaviorDesigner.Runtime;
using FMODUnity;

namespace FireRingStudio.BehaviourTree
{
    [System.Serializable]
    public class SharedStudioEventEmitter : SharedVariable<StudioEventEmitter>
    {
        public static implicit operator SharedStudioEventEmitter(StudioEventEmitter value) { return new SharedStudioEventEmitter { Value = value }; }
    }
}