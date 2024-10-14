using BehaviorDesigner.Runtime;

namespace FireRingStudio.BehaviourTree
{
    [System.Serializable]
    public class SharedIntReference : SharedVariable<UnityAtoms.BaseAtoms.IntReference>
    {
        public static implicit operator SharedIntReference(UnityAtoms.BaseAtoms.IntReference value) { return new SharedIntReference { Value = value }; }
    }
}