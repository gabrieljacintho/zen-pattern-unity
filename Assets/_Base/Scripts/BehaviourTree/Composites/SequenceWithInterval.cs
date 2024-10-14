using System;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public enum UpdateMode
    {
        FrameInterval,
        PerMinute
    }

    [TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. " +
                 "If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
    public class SequenceWithInterval : Sequence
    {
        [SerializeField] private UpdateMode _mode;
        [ShowIf("@_mode == UpdateMode.FrameInterval")]
        [SerializeField, Min(1)] private int _frameInterval = 1;
        [ShowIf("@_mode == UpdateMode.PerMinute")]
        [SerializeField] private SharedIntReference _updatesPerMinuteReference;

        private float _lastUpdateTime;

        private int UpdatesPerMinute => _updatesPerMinuteReference?.Value?.Value ?? 0;

        
        public override bool CanExecute()
        {
            if (!base.CanExecute())
            {
                return false;
            }
            
            switch (_mode)
            {
                case UpdateMode.FrameInterval:
                    return Time.frameCount % _frameInterval == 0;
                
                case UpdateMode.PerMinute:
                    float elapsedTime = Time.time - _lastUpdateTime; 
                    if (UpdatesPerMinute > 0 && (elapsedTime == 0f || elapsedTime >= 60f / UpdatesPerMinute))
                    {
                        _lastUpdateTime = Time.time;
                        return true;
                    }
                    return false;
                
                default:
                    return false;
            }
        }

        public override void OnReset()
        {
            base.OnReset();
            _frameInterval = 1;
            _updatesPerMinuteReference = default;
        }
    }
}