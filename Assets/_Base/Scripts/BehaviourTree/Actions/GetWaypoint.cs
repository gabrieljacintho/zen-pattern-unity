using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public class GetWaypoint : Action
    {
        [SerializeField] private SharedString _waypointsRootId;
        [SerializeField] private SharedVector3 _waypoint;

        private Transform _waypointRoot;
        private int _waypointIndex = -1;


        public override void OnStart()
        {
            if (_waypointRoot == null)
            {
                GameObject waypointRoot = GameObjectID.FindGameObjectWithID(_waypointsRootId.Value);
                if (waypointRoot != null)
                {
                    _waypointRoot = waypointRoot.transform;
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_waypointRoot == null)
            {
                return TaskStatus.Failure;
            }

            _waypointIndex++;

            if (_waypointIndex >= _waypointRoot.childCount)
            {
                return TaskStatus.Failure;
            }

            Transform waypoint = _waypointRoot.GetChild(_waypointIndex);

            _waypoint.Value = waypoint.position;

            return TaskStatus.Success;
        }
    }
}