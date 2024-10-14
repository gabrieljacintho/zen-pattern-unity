using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public class CustomRandomProbability : Conditional
    {
        [SerializeField] private SharedFloat _successProbability = 0.5f;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Set negative to not use.")]
        [SerializeField] private SharedInt _minFailures = -1;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Set negative to not use.")]
        [SerializeField] private SharedInt _maxFailures = -1;

        private int _failuresCount;


        public override TaskStatus OnUpdate()
        {
            if (_minFailures.Value > 0 && _failuresCount < _minFailures.Value)
            {
                _failuresCount++;
                return TaskStatus.Failure;
            }

            if (_maxFailures.Value > 0 && _failuresCount >= _maxFailures.Value)
            {
                _failuresCount = 0;
                return TaskStatus.Success;
            }

            float randomValue = UnityEngine.Random.Range(0f, 1f);
            if (randomValue <= _successProbability.Value)
            {
                _failuresCount = 0;
                return TaskStatus.Success;
            }

            _failuresCount++;
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _successProbability = 0.5f;
            _minFailures = -1;
            _maxFailures = -1;
            _failuresCount = 0;
        }
    }
}