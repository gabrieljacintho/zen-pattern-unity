using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Atom")]
    [TaskDescription("Performs a math operation on two atom floats: Add, Subtract, Multiply, Divide, Min, or Max.")]
    public class AtomFloatOperator : Action
    {
        public enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Min,
            Max,
            Modulo
        }

        [Tooltip("The operation to perform")]
        [SerializeField] private Operation _operation;
        [Tooltip("The first float")]
        [SerializeField] private SharedFloatReference _float1;
        [Tooltip("The second float")]
        [SerializeField] private SharedFloatReference _float2;
        [Tooltip("The variable to store the result")]
        [SerializeField] private SharedFloatReference _atomStoreResult;
        [SerializeField] private SharedFloat _storeResult;

        private float Value
        {
            set => SetValue(value);
        }


        public override TaskStatus OnUpdate()
        {
            switch (_operation)
            {
                case Operation.Add:
                    Value = _float1.Value.Value + _float2.Value.Value;
                    break;
                case Operation.Subtract:
                    Value = _float1.Value.Value - _float2.Value.Value;
                    break;
                case Operation.Multiply:
                    Value = _float1.Value.Value * _float2.Value.Value;
                    break;
                case Operation.Divide:
                    Value = _float1.Value.Value / _float2.Value.Value;
                    break;
                case Operation.Min:
                    Value = Mathf.Min(_float1.Value.Value, _float2.Value.Value);
                    break;
                case Operation.Max:
                    Value = Mathf.Max(_float1.Value.Value, _float2.Value.Value);
                    break;
                case Operation.Modulo:
                    Value = _float1.Value.Value % _float2.Value.Value;
                    break;
            }
            return TaskStatus.Success;
        }

        private void SetValue(float value)
        {
            if (_atomStoreResult != null)
            {
                _atomStoreResult.Value.Value = value;
            }

            _storeResult.Value = value;
        }

        public override void OnReset()
        {
            _operation = Operation.Add;
            _float1 = null;
            _float2 = null;
            _atomStoreResult = null;
            _storeResult = null;
        }
    }
}