using FireRingStudio.Operations;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentSpeedController : MonoBehaviour
    {
        [SerializeField] private FloatReference _speedReference = new (1f);
        [SerializeField] private FloatMultiplication _speedMultiplication;
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private FloatReference _minSpeedReference = new (0f);
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private FloatReference _maxSpeedReference = new (0f);
        
        private NavMeshAgent _agent;

        private float Speed => _speedReference?.Value ?? 0f;
        private float SpeedScale => _speedMultiplication.Result;
        private float MinSpeed => _minSpeedReference?.Value ?? 0;
        private float MaxSpeed => _maxSpeedReference?.Value ?? 0;
        
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            float newSpeed = Speed * SpeedScale;

            if (MinSpeed >= 0f)
            {
                newSpeed = Mathf.Max(newSpeed, MinSpeed);
            }

            if (MaxSpeed >= 0f)
            {
                newSpeed = Mathf.Min(newSpeed, MaxSpeed);
            }

            _agent.speed = newSpeed;
        }
    }
}