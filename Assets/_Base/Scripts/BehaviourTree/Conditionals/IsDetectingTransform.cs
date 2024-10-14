using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public class IsDetectingTransform : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedTransformList _targetTransformList;
        [SerializeField] private SharedVector3 _originOffset = Vector3.up;
        [SerializeField] private SharedVector3 _targetOffset = Vector3.up;
        [SerializeField] private SharedFloat _radius;
        [SerializeField] private SharedFloat _angle = 360f;
        [SerializeField] protected LayerMask _obstacleLayerMask;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction;
        [SerializeField] private SharedInt _updateFrameInterval = 1;
        [SerializeField] private SharedTransformList _detectedTransformListResult;
        [SerializeField] private SharedBool _showGizmos;

        private GameObject TargetGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;


        public override TaskStatus OnUpdate()
        {
            if (Time.frameCount % _updateFrameInterval.Value != 0 || _targetTransformList == null)
            {
                return TaskStatus.Running;
            }

            _detectedTransformListResult.Value.Clear();

            foreach (Transform transform in _targetTransformList.Value)
            {
                if (transform != null && CanDetect(transform))
                {
                    _detectedTransformListResult.Value.Add(transform);
                }
            }

            return _detectedTransformListResult.Value.Count > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool CanDetect(Transform targetTransform)
        {
            Vector3 originPosition = TargetGameObject.transform.position + _originOffset.Value;
            Vector3 targetPosition = targetTransform.transform.position + _targetOffset.Value;

            return Vector3.Distance(originPosition, targetPosition) <= _radius.Value
                && transform.CheckAngle(targetPosition, _angle.Value)
                && !PhysicsHelper.HasObstacleBetween(originPosition, targetPosition, _obstacleLayerMask, _queryTriggerInteraction);
        }

        public override void OnDrawGizmos()
        {
            if (!_showGizmos.Value)
            {
                return;
            }

            Gizmos.color = _detectedTransformListResult.Value.Count > 0 ? Color.red : Color.white;

            Transform transform = TargetGameObject.transform;
            Vector3 position = transform.position + _originOffset.Value;
            Gizmos.DrawWireSphere(position, _radius.Value);

            if (_angle.Value > 0 && _angle.Value < 360f)
            {
                GizmosHelper.DrawAngle(position, transform.eulerAngles, _angle.Value, _radius.Value);
            }
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _targetTransformList = null;
            _originOffset = Vector3.up;
            _targetOffset = Vector3.up;
            _radius = null;
            _angle = 360f;
            _obstacleLayerMask = default;
            _queryTriggerInteraction = default;
            _updateFrameInterval = 1;
            _detectedTransformListResult = null;
            _showGizmos = null;
        }
    }
}