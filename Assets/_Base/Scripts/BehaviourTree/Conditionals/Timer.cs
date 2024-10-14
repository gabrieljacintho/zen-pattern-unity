using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio")]
    public class Timer : Conditional
    {
        [SerializeField] private SharedFloat _minWaitTime;
        [SerializeField] private SharedFloat _maxWaitTime;
        [SerializeField] private SharedFloat _elapsedTime;
        [SerializeField] private TaskStatus _runningStatus = TaskStatus.Running;

        private float _waitTime = -1f;


        public override void OnStart()
        {
            if (_waitTime == -1f)
            {
                RandomWaitTime();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_elapsedTime.Value < _waitTime)
            {
                _elapsedTime.Value += Time.deltaTime;
                return _runningStatus;
            }

            RandomWaitTime();

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _minWaitTime = null;
            _maxWaitTime = null;
            _elapsedTime = null;
            _waitTime = default;
        }

        public void RandomWaitTime()
        {
            _waitTime = UnityEngine.Random.Range(_minWaitTime.Value, _maxWaitTime.Value);
        }
    }
}
