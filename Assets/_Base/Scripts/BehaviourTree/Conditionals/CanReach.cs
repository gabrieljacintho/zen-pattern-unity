using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Can the NavMeshAgent reach the target position? Returns success if true.")]
    public class CanReach : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedVector3 _targetPosition;
        [SerializeField] private SharedNavMeshPath _storePath;
        [SerializeField, Min(1)] private int _updateInterval = 1;

        private GameObject _previousGameObject;
        private NavMeshAgent _navMeshAgent;
        
        private TaskStatus _lastStatus = TaskStatus.Failure;
        
        
        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_navMeshAgent == null)
            {
                Debug.LogNull(nameof(NavMeshAgent));
                return TaskStatus.Failure;
            }
            
            if (Time.frameCount % _updateInterval != 0)
            {
                return _lastStatus;
            }

            if (_navMeshAgent.CanReach(_targetPosition.Value, out NavMeshPath path))
            {
                _storePath.Value = path;
                _lastStatus = TaskStatus.Success;
                return TaskStatus.Success;
            }

            _lastStatus = TaskStatus.Failure;
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _targetPosition = null;
        }
    }
}