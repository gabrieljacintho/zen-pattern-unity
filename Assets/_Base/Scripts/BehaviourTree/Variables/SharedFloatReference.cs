using BehaviorDesigner.Runtime;
using UnityAtoms.BaseAtoms;

namespace FireRingStudio.BehaviourTree
{
    [System.Serializable]
    public class SharedFloatReference : SharedVariable<FloatReference>
    {
        public static implicit operator SharedFloatReference(FloatReference value) { return new SharedFloatReference { Value = value }; }
    }
}