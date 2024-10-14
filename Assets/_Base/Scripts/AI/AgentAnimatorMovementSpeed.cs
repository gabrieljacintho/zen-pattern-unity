using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AgentAnimatorMovementSpeed : MonoBehaviour
    {
        [Serializable]
        public struct AgentSpeedAnimatorMovementSpeed
        {
            public FloatReference AgentSpeed;
            public FloatReference AnimatorMovementSpeed;
        }

        [SerializeField] private List<AgentSpeedAnimatorMovementSpeed> _values;
        [SerializeField] private bool _canScale;
        [ShowIf("_canScale")]
        [SerializeField] private float _maxAnimatorMovementSpeed = 3f;

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
            if (_values == null || _values.Count == 0)
            {
                return;
            }

            int closestValueIndex = GetClosestValueIndex();
            if (closestValueIndex == -1)
            {
                return;
            }

            int previousValueIndex;
            int nextValueIndex;

            if (_values[closestValueIndex].AgentSpeed >= _agent.speed)
            {
                nextValueIndex = closestValueIndex;

                if (closestValueIndex > 0)
                {
                    previousValueIndex = closestValueIndex - 1;
                    UpdateAnimatorMovementSpeed(previousValueIndex, nextValueIndex);
                }
                else
                {
                    _animator.SetFloat(s_movementSpeed, _values[nextValueIndex].AnimatorMovementSpeed);
                }
            }
            else
            {
                previousValueIndex = closestValueIndex;

                if (_values.Count > previousValueIndex + 1)
                {
                    nextValueIndex = closestValueIndex + 1;
                    UpdateAnimatorMovementSpeed(previousValueIndex, nextValueIndex);
                }
                else
                {
                    var value = _values[previousValueIndex];

                    float movementSpeed;
                    if (_canScale)
                    {
                        movementSpeed = _agent.speed * value.AnimatorMovementSpeed / value.AgentSpeed;
                        movementSpeed = Mathf.Min(movementSpeed, _maxAnimatorMovementSpeed);
                    }
                    else
                    {
                        movementSpeed = value.AnimatorMovementSpeed;
                    }

                    _animator.SetFloat(s_movementSpeed, movementSpeed);
                }
            }
        }

        private int GetClosestValueIndex()
        {
            int closestValueIndex = -1;

            for (int i = 0; i < _values.Count; i++)
            {
                if (closestValueIndex == -1)
                {
                    closestValueIndex = i;
                    continue;
                }

                float diffA = Mathf.Abs(_values[closestValueIndex].AgentSpeed.Value - _agent.speed);
                float diffB = Mathf.Abs(_values[i].AgentSpeed.Value - _agent.speed);
                if (diffB <= diffA)
                {
                    closestValueIndex = i;
                }
            }

            return closestValueIndex;
        }

        private void UpdateAnimatorMovementSpeed(int previousValueIndex, int nextValueIndex)
        {
            float previousAgentSpeed = _values[previousValueIndex].AgentSpeed;
            float t = (_agent.speed - previousAgentSpeed) / (_values[nextValueIndex].AgentSpeed - previousAgentSpeed);

            float movementSpeed = Mathf.Lerp(_values[previousValueIndex].AnimatorMovementSpeed, _values[nextValueIndex].AnimatorMovementSpeed, t);
            _animator.SetFloat(s_movementSpeed, movementSpeed);
        }
    }
}