using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Helpers
{
    public enum NavMeshQueryMethod
    {
        Nearest,
        Random,
        Reachable,
        Shortest
    }
    
    public static class NavMeshHelper
    {
        #region CanReach
        
        public static bool CanReach(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter filter, out NavMeshPath path)
        {
            path = new NavMeshPath();
            if (!NavMesh.CalculatePath(sourcePosition, targetPosition, filter, path))
            {
                return false;
            }
            
            return path.status == NavMeshPathStatus.PathComplete;
        }
        
        public static bool CanReach(Vector3 sourcePosition, Vector3 targetPosition, NavMeshQueryFilter filter)
        {
            return CanReach(sourcePosition, targetPosition, filter, out _);
        }
        
        #endregion
        
        public static bool NavMeshPosition(Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange, NavMeshQueryFilter filter, float heightTolerance = 0.2f)
        {
            float maxDistance = Mathf.Max(distanceRange.x, distanceRange.y);
            if (!NavMesh.SamplePosition(sourcePosition, out hit, maxDistance, filter))
            {
                return false;
            }

            float height = hit.position.y - sourcePosition.y;
            if (height > 0)
            {
                if (height > heightTolerance)
                {
                    return false;
                }
            }
            else if (height < 0f)
            {
                if (Mathf.Abs(height) > distanceRange.y)
                {
                    return false;
                }
            }
            
            Vector3 horizontalPosition = new Vector3(hit.position.x, sourcePosition.y, hit.position.z);
            if (Vector3.Distance(sourcePosition, horizontalPosition) > distanceRange.x)
            {
                return false;
            }

            return true;
        }
        
        #region RandomNavMeshPosition
        
        public static bool RandomNavMeshPosition(Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange, int areaMask,
            float maxDistance = 4f, bool invisible = false, int attempts = 32)
        {
            Camera mainCamera = invisible ? Camera.main : null;
            
            for (int i = 0; i < attempts; i++)
            {
                float randomDistance = UnityEngine.Random.Range(distanceRange.x, distanceRange.y);
                Vector3 randomPosition = sourcePosition + UnityEngine.Random.insideUnitSphere.normalized * randomDistance;

                // TODO: Separate distanceRange into horizontalDistanceRange and verticalDistanceRange
                randomPosition.y = Mathf.Clamp(randomPosition.y, sourcePosition.y - 100f, sourcePosition.y + 100f);

                if (!NavMesh.SamplePosition(randomPosition, out hit, maxDistance, areaMask))
                {
                    continue;
                }
                
                if (mainCamera != null)
                {
                    if (hit.position.IsVisibleByCamera(mainCamera))
                    {
                        continue;
                    }
                }
                
                return true;
            }

            hit = default;
            return false;
        }
        
        public static bool RandomNavMeshPosition(Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            NavMeshAgent agent, bool invisible = false, int attempts = 32)
        {
            float maxDistance = agent.height * 2f;
            
            return RandomNavMeshPosition(sourcePosition, out hit, distanceRange, agent.areaMask, maxDistance, invisible, attempts);
        }
        
        #endregion
        
        #region ReachableNavMeshPosition
        
        public static bool ReachableNavMeshPosition(Vector3 agentPosition, Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            NavMeshQueryFilter filter, out NavMeshPath path, float heightTolerance = 0.2f, int attempts = 32)
        {
            for (int i = 0; i < attempts; i++)
            {
                if (!NavMeshPosition(sourcePosition, out hit, distanceRange, filter, heightTolerance))
                {
                    continue;
                }
                
                if (!CanReach(agentPosition, hit.position, filter, out path))
                {
                    continue;
                }
                
                return true;
            }

            hit = default;
            path = default;
            return false;
        }
        
        public static bool ReachableNavMeshPosition(Vector3 agentPosition, Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            NavMeshQueryFilter filter, out NavMeshPath path, float maxDistance = 4f, bool invisible = false, int attempts = 32)
        {
            for (int i = 0; i < attempts; i++)
            {
                if (!RandomNavMeshPosition(sourcePosition, out hit, distanceRange, filter.areaMask, maxDistance, invisible, 1))
                {
                    continue;
                }
                
                if (!CanReach(agentPosition, hit.position, filter, out path))
                {
                    continue;
                }
                
                return true;
            }

            hit = default;
            path = default;
            return false;
        }

        public static bool ReachableNavMeshPosition(Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            NavMeshQueryFilter filter, out NavMeshPath path, float maxDistance = 4f, bool invisible = false, int attempts = 32)
        {
            return ReachableNavMeshPosition(sourcePosition, sourcePosition, out hit, distanceRange, filter, out path, maxDistance, invisible, attempts);
        }

        public static bool ReachableNavMeshPosition(NavMeshAgent agent, Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            out NavMeshPath path, bool invisible = false, int attempts = 32)
        {
            Vector3 agentPosition = agent.transform.position;
            NavMeshQueryFilter filter = agent.GetNavMeshQueryFilter();
            float maxDistance = agent.height * 2f;
            
            return ReachableNavMeshPosition(agentPosition, sourcePosition, out hit, distanceRange, filter, out path, maxDistance, invisible, attempts);
        }
        
        public static bool ReachableNavMeshPosition(NavMeshAgent agent, out NavMeshHit hit, Vector2 distanceRange,
            out NavMeshPath path, bool invisible = false, int attempts = 32)
        {
            Vector3 agentPosition = agent.transform.position;
            NavMeshQueryFilter filter = agent.GetNavMeshQueryFilter();
            float maxDistance = agent.height * 2f;
            
            return ReachableNavMeshPosition(agentPosition, agentPosition, out hit, distanceRange, filter, out path, maxDistance, invisible, attempts);
        }
        
        #endregion
        
        #region ShortestNavMeshPath

        public static bool ShortestNavMeshPath(Vector3 agentPosition, Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            NavMeshQueryFilter filter, out NavMeshPath path, float heightTolerance = 0.2f, int attempts = 32)
        {
            List<NavMeshPath> reachablePaths = new List<NavMeshPath>();
            Dictionary<NavMeshPath, NavMeshHit> hitByPath = new Dictionary<NavMeshPath, NavMeshHit>();
            for (int i = 0; i < attempts; i++)
            {
                if (ReachableNavMeshPosition(agentPosition, sourcePosition, out hit, distanceRange, filter, out path, heightTolerance,  1))
                {
                    reachablePaths.Add(path);
                    hitByPath.Add(path, hit);
                }
            }

            path = null;
            float closestLength = float.MaxValue;
            for (int i = 0; i < reachablePaths.Count; i++)
            {
                float length = reachablePaths[i].GetLength();
                if (length < closestLength)
                {
                    path = reachablePaths[i];
                    closestLength = length;
                }
            }

            hit = path != null ? hitByPath[path] : default;

            return path != null;
        }

        public static bool ShortestNavMeshPath(NavMeshAgent agent, Vector3 sourcePosition, out NavMeshHit hit, Vector2 distanceRange,
            out NavMeshPath path, float heightTolerance = 0.2f, int attempts = 32)
        {
            Vector3 agentPosition = agent.transform.position;
            NavMeshQueryFilter filter = agent.GetNavMeshQueryFilter();

            return ShortestNavMeshPath(agentPosition, sourcePosition, out hit, distanceRange, filter, out path, heightTolerance, attempts);
        }
        
        #endregion
    }
}