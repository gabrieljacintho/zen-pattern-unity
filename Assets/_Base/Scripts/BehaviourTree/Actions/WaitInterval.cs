using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskDescription("Wait a interval. The task will return running until the task is done waiting. It will return success after the interval has elapsed.")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    public class WaitInterval : Action
    {
        [SerializeField] private SharedIntReference _successesPerMinute;

        private float _remainingTime;

        private bool _isPaused;

        private int SuccessesPerMinute => _successesPerMinute?.Value?.Value ?? 0;


        public override void OnStart()
        {
            _remainingTime = SuccessesPerMinute > 0 ? 60f / SuccessesPerMinute : 0f;
        }

        public override TaskStatus OnUpdate()
        {
            if (_remainingTime <= 0f)
            {
                return TaskStatus.Success;
            }

            if (!_isPaused)
            {
                _remainingTime -= Time.deltaTime;
            }

            return TaskStatus.Running;
        }

        public override void OnPause(bool paused)
        {
            _isPaused = paused;
        }

        public override void OnReset()
        {
            _successesPerMinute = null;
        }
    }
}