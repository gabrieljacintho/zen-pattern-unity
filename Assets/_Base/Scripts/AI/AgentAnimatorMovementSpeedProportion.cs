using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Assets._AssetBase.Scripts.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AgentAnimatorMovementSpeedProportion : MonoBehaviour
    {
        [SerializeField] private float _proportion = 1f;

        private NavMeshAgent _agent;
        private Animator _animator;

        private static readonly int s_movementSpeed = Animator.StringToHash("MovementSpeed");


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            _animator.SetFloat(s_movementSpeed, _agent.speed * _proportion);
        }
    }
}