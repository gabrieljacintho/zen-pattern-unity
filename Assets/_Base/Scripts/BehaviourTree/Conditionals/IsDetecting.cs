using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/Physics")]
    [TaskDescription("Is the GameObject detecting something? Returns success if true.")]
    public class IsDetecting : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedVector3 _originOffset = Vector3.up;
        [SerializeField] private SharedFloat _radius;
        [SerializeField] private SharedFloat _angle = 360f;
        [SerializeField] private LayerMask _targetLayerMask = ~0;
        [SerializeField] protected LayerMask _obstacleLayerMask;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction;
        [SerializeField] protected SharedString _requiredTag;
        [SerializeField] private SharedInt _updateFrameInterval = 1;
        [SerializeField] private SharedColliderList _detectedColliderListResult;
        [SerializeField] private SharedBool _showGizmos;

        private GameObject TargetGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;


        public override TaskStatus OnUpdate()
        {
            if (Time.frameCount % _updateFrameInterval.Value != 0)
            {
                return TaskStatus.Running;
            }

            Vector3 position = TargetGameObject.transform.position + _originOffset.Value;
            Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, _radius.Value, _targetLayerMask, _queryTriggerInteraction);

            _detectedColliderListResult.Value.Clear();

            foreach (Collider collider in colliders)
            {
                if (CanDetect(collider))
                {
                    _detectedColliderListResult.Value.Add(collider);
                }
            }

            return _detectedColliderListResult.Value.Count > 0 ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool CanDetect(Collider targetCollider)
        {
            Transform transform = TargetGameObject.transform;

            return transform.CheckAngle(targetCollider.transform.position, _angle.Value)
                && PhysicsHelper.CanDetect(targetCollider.gameObject, _targetLayerMask, _requiredTag.Value)
                && !PhysicsHelper.HasObstacleBetween(transform.position, targetCollider, _obstacleLayerMask, _queryTriggerInteraction);
        }

        public override void OnDrawGizmos()
        {
            if (!_showGizmos.Value)
            {
                return;
            }

            Gizmos.color = _detectedColliderListResult.Value.Count > 0 ? Color.red : Color.white;

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
            _originOffset = Vector3.up;
            _radius = null;
            _angle = 360f;
            _targetLayerMask = ~0;
            _obstacleLayerMask = default;
            _queryTriggerInteraction = default;
            _requiredTag = null;
            _updateFrameInterval = 1;
            _detectedColliderListResult = null;
            _showGizmos = null;
        }
    }
}