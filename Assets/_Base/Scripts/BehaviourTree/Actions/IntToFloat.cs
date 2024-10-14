using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public class IntToFloat : Action
    {
        [SerializeField] private SharedInt _intValue;
        [SerializeField] private SharedFloat _storeFloat;


        public override TaskStatus OnUpdate()
        {
            _storeFloat.Value = _intValue.Value;

            return TaskStatus.Success;
        }
    }
}