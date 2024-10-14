using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/Behaviour")]
    public class IsBehaviourNull : Conditional
    {
        [SerializeField] private SharedBehaviour _target;


        public override TaskStatus OnUpdate()
        {
            return _target == null || _target.Value == null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _target = null;
        }
    }
}