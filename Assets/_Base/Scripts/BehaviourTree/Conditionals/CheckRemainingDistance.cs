using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Checks the distance between the agent's position and the destination on the current path. Returns success if within the maximum limit..")]
    public class CheckRemainingDistance : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedBool _checkMinDistance;
        [SerializeField] private SharedFloat _minDistance;
        [SerializeField] private SharedBool _checkMaxDistance;
        [SerializeField] private SharedFloat _maxDistance;

        private GameObject _previousGameObject;
        private NavMeshAgent _navMeshAgent;


        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_previousGameObject);
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

            bool result = true;
            float remainingDistance = _navMeshAgent.remainingDistance;

            if (_checkMinDistance.Value)
            {
                result &= remainingDistance >= _minDistance.Value;
            }

            if (_checkMaxDistance.Value)
            {
                result &= remainingDistance <= _maxDistance.Value;
            }

            return result ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _checkMinDistance.Value = default;
            _minDistance.Value = default;
            _checkMaxDistance.Value = default;
            _maxDistance.Value = default;
        }
    }
}