using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("FireRing Studio/Pool")]
    [TaskDescription("Release the GameObject to Pool. Returns success.")]
    public class ReleaseToPool : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedString _poolTag;
        [SerializeField] private SharedFloat _delay;
        [SerializeField] private SharedBool _inRealtime;


        public override TaskStatus OnUpdate()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);

            if (_poolTag != null && _poolTag.Value != null)
            {
                if (_delay != null && _delay.Value > 0f)
                {
                    bool inRealtime = _inRealtime != null && _inRealtime.Value;
                    currentGameObject.ReleaseToPool(_poolTag.Value, _delay.Value, inRealtime);

                }
                else
                {
                    currentGameObject.ReleaseToPool(_poolTag.Value);
                }
            }
            else
            {
                if (_delay != null && _delay.Value > 0f)
                {
                    bool inRealtime = _inRealtime != null && _inRealtime.Value;
                    currentGameObject.ReleaseToPool(_delay.Value, inRealtime);
                }
                else
                {
                    currentGameObject.ReleaseToPool();
                }
            }
            
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _poolTag = null;
            _delay = null;
            _inRealtime = null;
        }
    }
}