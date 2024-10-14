using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Cache;
using FireRingStudio.Vitals;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/Collider")]
    [TaskDescription("Get the closest Collider in a list. Returns Success if found.")]
    public class GetClosestCollider : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedColliderList _colliderList;
        [SerializeField] private SharedCollider _colliderResult;
        [SerializeField] private SharedGameObject _gameObjectResult;
        [SerializeField] private SharedVector3 _positionResult;

        private GameObject TargetGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;


        public override TaskStatus OnUpdate()
        {
            if (_colliderList.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }

            Collider closestCollider = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider collider in _colliderList.Value)
            {
                Vector3 positionA = TargetGameObject.transform.position;
                Vector3 positionB = collider.transform.position;
                float distance = Vector3.Distance(positionA, positionB);

                if (closestCollider == null || distance < closestDistance)
                {
                    closestCollider = collider;
                    closestDistance = distance;
                }
            }

            _colliderResult.Value = closestCollider;
            _gameObjectResult.Value = closestCollider.gameObject;
            _positionResult.Value = closestCollider.transform.position;

            return _colliderResult.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _colliderList.Value = null;
            _colliderResult.Value = null;
            _gameObjectResult.Value = null;
            _positionResult.Value = default;
        }
    }
}