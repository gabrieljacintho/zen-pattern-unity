using BehaviorDesigner.Runtime;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [System.Serializable]
    public class SharedNavMeshPath : SharedVariable<NavMeshPath>
    {
        public static implicit operator SharedNavMeshPath(NavMeshPath value) { return new SharedNavMeshPath { Value = value }; }
    }
}