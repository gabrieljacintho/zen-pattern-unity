using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Unity/Renderer")]
    [TaskDescription("Returns success if any child renderers are visible, otherwise return failure.")]
    public class IsChildrenVisible : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedBool _checkAny;
        [SerializeField] private SharedBool _invert;

        private GameObject _previousGameObject;
        private Renderer[] _renderers;

        
        public override void OnStart()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _renderers = currentGameObject.GetComponentsInChildren<Renderer>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_renderers == null)
            {
                Debug.LogNull(nameof(Renderer));
                return TaskStatus.Failure;
            }

            bool result;
            if (_checkAny != null && _checkAny.Value)
            {
                result = _renderers.Any(x => x.isVisible);
            }
            else
            {
                result = _renderers.All(x => x.isVisible);
            }

            if (_invert != null && _invert.Value)
            {
                result = !result;
            }

            return result ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
        }
    }
}