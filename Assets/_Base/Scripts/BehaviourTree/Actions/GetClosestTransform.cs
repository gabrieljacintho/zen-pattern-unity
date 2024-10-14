using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public class GetClosestTransform : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedTransformList _transformList;
        [SerializeField] private SharedTransform _transformResult;
        [SerializeField] private SharedVector3 _positionResult;

        private GameObject TargetGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;


        public override TaskStatus OnUpdate()
        {
            if (_transformList.Value.Count == 0)
            {
                return TaskStatus.Failure;
            }

            Transform closestTransform = null;
            float closestDistance = Mathf.Infinity;
            foreach (Transform transform in _transformList.Value)
            {
                Vector3 positionA = TargetGameObject.transform.position;
                Vector3 positionB = transform.position;
                float distance = Vector3.Distance(positionA, positionB);

                if (closestTransform == null || distance < closestDistance)
                {
                    closestTransform = transform;
                    closestDistance = distance;
                }
            }

            _transformResult.Value = closestTransform;
            _positionResult.Value = _transformResult.Value != null ? _transformResult.Value.position : default;

            return _transformResult.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _transformList.Value = null;
            _transformResult.Value = null;
        }
    }
}