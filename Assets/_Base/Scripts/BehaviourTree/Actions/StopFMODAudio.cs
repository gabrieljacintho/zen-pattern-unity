using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/FMOD")]
    [TaskDescription("Stop a FMOD event emitter. Returns success.")]
    public class StopFMODAudio : Action
    {
        [SerializeField] private SharedStudioEventEmitter _eventEmitter;
        [SerializeField] private SharedBool _changeAllowFadeout;
        [SerializeField] private SharedBool _allowFadeout;


        public override TaskStatus OnUpdate()
        {
            if (_eventEmitter.Value == null)
            {
                return TaskStatus.Success;
            }

            bool allowFadeout = _eventEmitter.Value.AllowFadeout;
            if (_changeAllowFadeout.Value)
            {
                _eventEmitter.Value.AllowFadeout = _allowFadeout.Value;
            }

            _eventEmitter.Value.Stop();

            if (_changeAllowFadeout.Value)
            {
                _eventEmitter.Value.AllowFadeout = allowFadeout;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _eventEmitter = null;
            _changeAllowFadeout = null;
            _allowFadeout = null;
        }
    }
}