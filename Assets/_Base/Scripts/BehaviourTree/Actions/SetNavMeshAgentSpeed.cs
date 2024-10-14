using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Sets the maximum movement speed when following a path. Returns success.")]
    public class SetNavMeshAgentSpeed : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedFloat _value;
        [SerializeField] private SharedBool _useAtom = true;
        [Tooltip("The NavMeshAgent speed")]
        [SerializeField] private SharedFloatReference _speed;

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

            if (_useAtom.Value)
            {
                _navMeshAgent.speed = _speed.Value.Value;
            }
            else
            {
                _navMeshAgent.speed = _value.Value;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _speed.Value = null;
        }
    }
}