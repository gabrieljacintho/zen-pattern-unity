using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Conditionals
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Does NavMeshAgent have a path? Returns success if true.")]
    public class HasPath : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedBool _checkIfIsNotPending;
        [SerializeField] private SharedBool _checkPathStatus;
        [SerializeField] private NavMeshPathStatus _requiredPathStatus;
        [SerializeField] private SharedBool _checkIfIsNotStale;

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

            if (_checkIfIsNotPending.Value && _navMeshAgent.pathPending)
            {
                return TaskStatus.Failure;
            }

            bool result = _navMeshAgent.hasPath;

            if (_checkPathStatus.Value)
            {
                result &= _navMeshAgent.pathStatus == _requiredPathStatus;
            }

            if (_checkIfIsNotStale.Value)
            {
                result &= !_navMeshAgent.isPathStale;
            }

            return result ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _checkPathStatus.Value = default;
            _requiredPathStatus = default;
            _checkIfIsNotStale.Value = default;
        }
    }
}