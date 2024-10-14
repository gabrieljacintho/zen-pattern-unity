using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/Collider")]
    [TaskDescription("Get the Collider game object. Returns Success if found.")]
    public class GetColliderGameObject : Action
    {
        [SerializeField] private SharedCollider _targetCollider;
        [SerializeField] private SharedGameObject _gameObjectResult;


        public override TaskStatus OnUpdate()
        {
            _gameObjectResult.Value = _targetCollider.Value != null ? _targetCollider.Value.gameObject : null;

            return _gameObjectResult.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetCollider.Value = null;
            _gameObjectResult.Value = null;
        }
    }
}