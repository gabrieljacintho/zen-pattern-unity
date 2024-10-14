using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/GameObject")]
    public class IsNull : Conditional
    {
        [SerializeField] private SharedGameObject _targetGameObject;


        public override TaskStatus OnUpdate()
        {
            return _targetGameObject == null || _targetGameObject.Value == null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
        }
    }
}