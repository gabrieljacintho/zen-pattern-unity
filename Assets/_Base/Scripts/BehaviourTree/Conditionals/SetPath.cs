using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Assign the NavMeshPath to the NavMeshAgent. Returns success if the path is assigned.")]
    public class SetPath : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField, RequiredField] private SharedNavMeshPath _path;

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

            return _navMeshAgent.path == _path.Value || _navMeshAgent.SetPath(_path.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _path = null;
        }
    }
}