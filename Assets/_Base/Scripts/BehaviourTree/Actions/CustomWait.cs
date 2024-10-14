using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    public class CustomWait : Action
    {
        [SerializeField] private TimeMeasure _measure;
        [SerializeField] private SharedFloat _nonAtomWaitTime;
        [SerializeField] private SharedBool _useAtom = true;
        [Tooltip("The amount of time to wait")]
        [SerializeField] private SharedFloatReference _waitTime;
        [Tooltip("Should the wait be randomized?")]
        [SerializeField] private SharedBool _randomWait = false;
        [Tooltip("The minimum wait time if random wait is enabled")]
        [SerializeField] private SharedFloatReference _minWaitTime;
        [Tooltip("The maximum wait time if random wait is enabled")]
        [SerializeField] private SharedFloatReference _maxWaitTime;

        private float _remainingTime;
        
        private bool _isPaused;

        
        public override void OnStart()
        {
            if (_randomWait.Value)
            {
                _remainingTime = UnityEngine.Random.Range(_minWaitTime.Value, _maxWaitTime.Value);
            }
            else
            {
                _remainingTime = _useAtom.Value ? _waitTime.Value : _nonAtomWaitTime.Value;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_remainingTime <= 0f)
            {
                return TaskStatus.Success;
            }
            
            if (!_isPaused)
            {
                switch (_measure)
                {
                    case TimeMeasure.Seconds:
                        _remainingTime -= Time.deltaTime;
                        break;
                    
                    case TimeMeasure.Frames:
                        _remainingTime--;
                        break;
                }
            }
            
            return TaskStatus.Running;
        }

        public override void OnPause(bool paused)
        {
            _isPaused = paused;
        }

        public override void OnReset()
        {
            _measure = default;
            _waitTime = default;
            _randomWait = false;
            _minWaitTime = null;
            _maxWaitTime = null;
        }
    }
}