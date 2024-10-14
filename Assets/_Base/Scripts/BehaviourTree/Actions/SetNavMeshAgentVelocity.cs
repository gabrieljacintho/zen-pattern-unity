using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Sets the nav mesh agent velocity. Returns success.")]
    public class SetNavMeshAgentVelocity : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [Tooltip("The NavMeshAgent speed")]
        [SerializeField] private SharedVector3 _velocity;

        private NavMeshAgent _navMeshAgent;
        private GameObject _previousGameObject;


        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
                _previousGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_navMeshAgent == null)
            {
                Debug.LogNull(nameof(NavMeshAgent));
                return TaskStatus.Failure;
            }

            _navMeshAgent.velocity = _velocity.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _velocity = null;
        }
    }
}