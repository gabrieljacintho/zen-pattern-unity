using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the difference of the variable value to the compareTo value is less than or equal to the maxValue.")]
    public class CustomCompareSharedQuaternion : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to compare.")]
        [SerializeField] private SharedQuaternion _variable;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The variable to compare to.")]
        [SerializeField] private SharedQuaternion _compareTo;
        [SerializeField] private SharedFloat _maxValue;

        
        public override TaskStatus OnUpdate()
        {
            float value = Quaternion.Angle(_variable.Value, _compareTo.Value);
            
            return value <= _maxValue.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _variable = Quaternion.identity;
            _compareTo = Quaternion.identity;
            _maxValue = 0f;
        }
    }
}