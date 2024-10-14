using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Is the NavMeshAgent at the destination? Returns success if true.")]
    public class IsAtDestination : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private NavMeshPathStatus _requiredPathStatus = NavMeshPathStatus.PathComplete;

        private GameObject _previousGameObject;
        private NavMeshAgent _navMeshAgent;

        
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

            return _navMeshAgent.IsAtDestination(_requiredPathStatus) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _requiredPathStatus = NavMeshPathStatus.PathComplete;
        }
    }
}