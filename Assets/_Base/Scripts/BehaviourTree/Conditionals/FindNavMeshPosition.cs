using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Finds a NavMesh position. Returns success if found.")]
    public class FindNavMeshPosition : Conditional
    {
        [SerializeField] private NavMeshQueryMethod _method;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedBool _useCustomAgentPosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("If null the target GameObject is used.")]
        [SerializeField] private SharedVector3 _agentPosition;
        [SerializeField] private SharedBool _useCustomSourcePosition;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("If null the target GameObject is used.")]
        [SerializeField] private SharedVector3 _sourcePosition;
        [SerializeField] private SharedVector2 _distanceRange;
        [SerializeField, Min(0f)] private float _heightTolerance = 0.2f;
        [SerializeField] private bool _invisible;
        [SerializeField, Min(1)] private int _attempts = 32;
        [SerializeField] private SharedVector3 _storePosition;
        [SerializeField] private SharedNavMeshPath _storePath;

        private GameObject _previousGameObject;
        private NavMeshAgent _agent;

        public GameObject TargetGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;
        public Vector3 AgentPosition => _useCustomAgentPosition.Value ? _agentPosition.Value : TargetGameObject.transform.position;
        public Vector3 SourcePosition => _useCustomSourcePosition.Value ? _sourcePosition.Value : TargetGameObject.transform.position;
        
        
        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _agent = currentGameObject.GetComponent<NavMeshAgent>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_agent == null)
            {
                Debug.LogNull(nameof(NavMeshAgent));
                return TaskStatus.Failure;
            }

            NavMeshHit hit;
            NavMeshPath path;

            switch (_method)
            {
                case NavMeshQueryMethod.Nearest:
                    NavMeshQueryFilter filter = _agent.GetNavMeshQueryFilter();
                    if (NavMeshHelper.NavMeshPosition(SourcePosition, out hit, _distanceRange.Value, filter, _heightTolerance))
                    {
                        _storePosition.Value = hit.position;
                        return TaskStatus.Success;
                    }
                    break;
                
                case NavMeshQueryMethod.Random:
                    if (NavMeshHelper.RandomNavMeshPosition(SourcePosition, out hit, _distanceRange.Value, _agent.areaMask,
                            _agent.height, _invisible, _attempts))
                    {
                        _storePosition.Value = hit.position;
                        return TaskStatus.Success;
                    }
                    break;
                
                case NavMeshQueryMethod.Reachable:
                    if (NavMeshHelper.ReachableNavMeshPosition(_agent, SourcePosition, out hit, _distanceRange.Value, out path,
                            _invisible, _attempts))
                    {
                        _storePosition.Value = hit.position;
                        _storePath.Value = path;
                        return TaskStatus.Success;
                    }
                    break;
                
                case NavMeshQueryMethod.Shortest:
                    if (NavMeshHelper.ShortestNavMeshPath(_agent, SourcePosition, out hit, _distanceRange.Value, out path,
                            _heightTolerance, _attempts))
                    {
                        _storePosition.Value = hit.position;
                        _storePath.Value = path;
                        return TaskStatus.Success;
                    }
                    break;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _method = NavMeshQueryMethod.Nearest;
            _targetGameObject = null;
            _sourcePosition = null;
            _distanceRange = null;
            _invisible = false;
            _heightTolerance = 0.2f;
            _attempts = 32;
            _storePosition = null;
            _storePath = null;
        }
    }
}