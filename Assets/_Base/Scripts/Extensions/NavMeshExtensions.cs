using FireRingStudio.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Extensions
{
    public static class NavMeshExtensions
    {
        public static bool IsAtDestination(this NavMeshAgent agent, NavMeshPathStatus requiredPathStatus = NavMeshPathStatus.PathComplete)
        {
            if (!agent.hasPath || agent.pathPending)
            {
                return false;
            }

            if (requiredPathStatus != NavMeshPathStatus.PathInvalid)
            {
                NavMeshPathStatus pathStatus = agent.pathStatus;
                if (requiredPathStatus == NavMeshPathStatus.PathPartial)
                {
                    if (pathStatus == NavMeshPathStatus.PathInvalid)
                    {
                        return false;
                    }
                }
                else if (pathStatus != NavMeshPathStatus.PathComplete)
                {
                    return false;
                }
            }

            return agent.remainingDistance <= agent.stoppingDistance;
        }
        
        public static bool CanReach(this NavMeshAgent agent, Vector3 sourcePosition, Vector3 targetPosition, out NavMeshPath path)
        {
            NavMeshQueryFilter filter = agent.GetNavMeshQueryFilter();

            return NavMeshHelper.CanReach(sourcePosition, targetPosition, filter, out path);
        }
        
        public static bool CanReach(this NavMeshAgent agent, Vector3 targetPosition, out NavMeshPath path)
        {
            return agent.CanReach(agent.transform.position, targetPosition, out path);
        }
        
        public static NavMeshQueryFilter GetNavMeshQueryFilter(this NavMeshAgent agent)
        {
            NavMeshQueryFilter filter = new NavMeshQueryFilter();
            filter.agentTypeID = agent.agentTypeID;
            filter.areaMask = agent.areaMask;

            return filter;
        }

        public static float GetLength(this NavMeshPath path)
        {
            float length = 0f;
            Vector3[] corners = path.corners;
            
            for (int i = 0; i < corners.Length - 1; i++)
            {
                length += Vector3.Distance(corners[i], corners[i + 1]);
            }

            return length;
        }
    }
}