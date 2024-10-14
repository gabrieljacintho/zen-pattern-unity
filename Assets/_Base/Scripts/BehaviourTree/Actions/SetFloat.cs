using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Atom")]
    public class SetFloat : Action
    {
        [SerializeField] private SharedFloatReference _floatValue;
        [SerializeField] private SharedFloat _storeFloat;


        public override TaskStatus OnUpdate()
        {
            _storeFloat.Value = _floatValue.Value.Value;

            return TaskStatus.Success;
        }
    }
}
