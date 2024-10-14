using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/Transform")]
    [TaskDescription("Rotates the transform towards the target. Returns success if completed.")]
    public class RotateTowardsGameObject : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [Tooltip("The GameObject to look at. If null the target position is used.")]
        [SerializeField] private SharedGameObject _targetLookAt;
        [SerializeField] private SharedVector3 _targetPosition;
        [SerializeField] private SharedVector3 _upwards = Vector3.up;
        [SerializeField] private SharedFloat _maxDegreesDelta = 360f;
        [SerializeField] private SharedFloat _successAngleDistance = 5f;
        [SerializeField] private SharedQuaternion _targetRotationResult;

        private GameObject _previousGameObject;
        private Transform _targetTransform;

        
        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _targetTransform = currentGameObject.transform;
            }
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 targetPosition = _targetLookAt.Value != null ? _targetLookAt.Value.transform.position : _targetPosition.Value;
            Vector3 forward = targetPosition - _targetTransform.position;

            Quaternion currentRotation = _targetTransform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(forward, _upwards.Value);
            targetRotation.x = currentRotation.x;
            targetRotation.z = currentRotation.z;

            _targetTransform.rotation = Quaternion.RotateTowards(_targetTransform.rotation, targetRotation, _maxDegreesDelta.Value * Time.deltaTime);
            _targetRotationResult.Value = targetRotation;

            if (_successAngleDistance.Value >= 360)
            {
                return TaskStatus.Success;
            }

            float angleDistance = Mathf.Abs(_targetTransform.rotation.eulerAngles.y - targetRotation.eulerAngles.y);

            return angleDistance <= _successAngleDistance.Value ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _targetLookAt.Value = null;
            _upwards.Value = Vector3.up;
            _maxDegreesDelta.Value = 360f;
        }
    }
}