using System.Collections;
using FireRingStudio.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.AI
{
    [RequireComponent(typeof(NavMeshObstacle), typeof(NavMeshAgent))]
    public class NavMeshObstacleAgent : MonoBehaviour
    {
        public NavMeshObstacle Obstacle { get; private set; }
        public NavMeshAgent Agent { get; private set; }


        private void Awake()
        {
            Obstacle = GetComponent<NavMeshObstacle>();
            Agent = GetComponent<NavMeshAgent>();
        }

        [ShowIf("@Agent != null && Agent.enabled")]
        [Button]
        public void EnableObstacle()
        {
            if (Agent != null)
                Agent.enabled = false;
            
            if (Obstacle != null)
                Obstacle.enabled = true;
            
            StopAllCoroutines();
        }

        [ShowIf("@Obstacle != null && Obstacle.enabled")]
        [Button]
        public void EnableAgent() => StartCoroutine(EnableAgentRoutine());

        private IEnumerator EnableAgentRoutine()
        {
            if (Obstacle != null)
                Obstacle.enabled = false;

            yield return null;

            if (Agent != null)
                Agent.enabled = true;
        }
    }
}
