using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.AI;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/NavMesh")]
    public class NavMeshRaycast : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedVector3 _sourcePosition;
        [SerializeField] private Vector3 _direction;
        [SerializeField, Min(0f)] private float _maxDistance = 1f;
        [SerializeField, Min(1)] private int _updateInterval = 1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Stores the hit point of the raycast")]
        [SerializeField] private SharedVector3 _storeHitPoint;

        private GameObject _previousGameObject;
        private NavMeshAgent _navMeshAgent;
        private NavMeshQueryFilter _filter;
        
        private TaskStatus _lastStatus = TaskStatus.Failure;
        
        
        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();

                if (_navMeshAgent != null)
                {
                    _filter = _navMeshAgent.GetNavMeshQueryFilter();
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_navMeshAgent == null)
            {
                Debug.LogNull(nameof(NavMeshAgent));
                _lastStatus = TaskStatus.Failure;
                return TaskStatus.Failure;
            }
            
            if (UpdateManager.FixedUpdateCount % _updateInterval != 0)
            {
                return _lastStatus;
            }

            Vector3 targetPosition = _sourcePosition.Value + _direction.normalized * _maxDistance;

            if (NavMesh.Raycast(_sourcePosition.Value, targetPosition, out NavMeshHit hit, _filter))
            {
                _storeHitPoint.Value = hit.position;
                _lastStatus = TaskStatus.Success;
                return TaskStatus.Success;
            }

            _lastStatus = TaskStatus.Failure;
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _sourcePosition = null;
            _direction = Vector3.zero;
            _maxDistance = 1f;
            _updateInterval = 1;
            _storeHitPoint = null;
        }
    }
}