using FireRingStudio.Helpers;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AgentAnimatorMovementAmount : MonoBehaviour
    {
        [SerializeField] private bool _hasWalkAnimation = true;
        [ShowIf("_hasWalkAnimation")]
        [Tooltip("The equivalent speed of the agent in the walking animation.")]
        [SerializeField] private FloatVariable _agentWalkSpeed;
        [SerializeField] private bool _hasRunAnimation = true;
        [ShowIf("_hasRunAnimation")]
        [Tooltip("The equivalent speed of the agent in the running animation.")]
        [SerializeField] private FloatVariable _agentRunSpeed;
        [SerializeField] private float _smoothTime = 0.3f;
        
        private NavMeshAgent _agent;
        private Animator _animator;

        private float _velocity;
        
        private static readonly int s_movementAmount = Animator.StringToHash("MovementAmount");


        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            float currentSpeed = _agent.velocity.magnitude;

            float movementAmount = 0f;

            if (_hasWalkAnimation && (currentSpeed <= _agentWalkSpeed.Value || !_hasRunAnimation))
            {
                float t = Mathf.Clamp01(currentSpeed / _agentWalkSpeed.Value);
;               movementAmount = Mathf.Lerp(0f, 0.5f, t);
            }
            else if (_hasRunAnimation)
            {
                float t;
                if (_hasWalkAnimation)
                {
                    t = (currentSpeed - _agentWalkSpeed.Value) / (_agentRunSpeed.Value - _agentWalkSpeed.Value);
                }
                else
                {
                    t = currentSpeed / _agentRunSpeed.Value;
                }

                t = Mathf.Clamp01(t);
                movementAmount = Mathf.Lerp(0.5f, 1f, t);
            }

            if (_smoothTime > 0f)
            {
                float currentMovementAmount = _animator.GetFloat(s_movementAmount);
                movementAmount = Mathf.SmoothDamp(currentMovementAmount, movementAmount, ref _velocity, _smoothTime);
            }

            _animator.SetFloat(s_movementAmount, movementAmount);
        }
    }
}