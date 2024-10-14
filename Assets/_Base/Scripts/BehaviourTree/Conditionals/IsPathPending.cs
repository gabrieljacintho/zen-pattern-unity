using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Is the NavMeshAgent path pending? Returns success if true.")]
    public class IsPathPending : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;

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

            return _navMeshAgent.pathPending ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
        }
    }
}