using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using FireRingStudio.Pool;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Spawn
{
    public class NavMeshSpawner : Spawner
    {
        [Header("Extra Settings")]
        [SerializeField] private bool _reachableSpawnPoint;
        [SerializeField, Min(0f)] private float _defaultAgentHeight = 2f;
        [SerializeField] private string _defaultNavMeshAreaName = "Walkable";

        private Dictionary<GameObject, NavMeshAgent> _agents = new Dictionary<GameObject, NavMeshAgent>();
        
        private int _defaultNavMeshArea;

        
        protected override void Awake()
        {
            base.Awake();
            
            _defaultNavMeshArea = 1 << NavMesh.GetAreaFromName(_defaultNavMeshAreaName);
        }
        
        protected override bool TryGetSpawnPoint(GameObject prefab, bool invisible, out Vector3 point, out Quaternion rotation, out List<PooledObject> releasedObjects)
        {
            point = transform.position;
            rotation = Quaternion.identity;
            releasedObjects = new List<PooledObject>();

            if (!_agents.TryGetValue(prefab, out NavMeshAgent agent))
            {
                agent = prefab.GetComponent<NavMeshAgent>();
                _agents.Add(prefab, agent);
            }

            NavMeshHit hit;
            NavMeshQueryFilter filter;
            float agentHeight;
            
            if (agent != null)
            {
                filter = agent.GetNavMeshQueryFilter();
                agentHeight = agent.height * 2f;
            }
            else
            {
                filter = new NavMeshQueryFilter
                {
                    areaMask = _defaultNavMeshArea
                };
                agentHeight = _defaultAgentHeight;
            }

            if (_reachableSpawnPoint)
            {
                Vector2 distanceRange = new Vector2(4f, 4f);
                if (NavMeshHelper.NavMeshPosition(point, out hit, distanceRange, filter, 1f))
                {
                    point = hit.position;
                }
                else
                {
                    return false;
                }
                
                if (!NavMeshHelper.ReachableNavMeshPosition(point, out hit, SpawnDistanceRange, filter, out _, agentHeight, invisible, 1))
                {
                    return false;
                }
            }
            else
            {
                if (!NavMeshHelper.RandomNavMeshPosition(point, out hit, SpawnDistanceRange, filter.areaMask, agentHeight, invisible, 1))
                {
                    return false;
                }
            }
            
            point = hit.position;
            return CanSpawnAt(point, out releasedObjects);
        }

    }
}