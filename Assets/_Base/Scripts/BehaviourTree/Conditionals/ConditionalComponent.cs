using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    public abstract class ConditionalComponent<T> : Conditional where T : Component
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] protected SharedGameObject _targetGameObject;
        [SerializeField] protected SharedBool _targetCanBeNull;

        protected GameObject _previousGameObject;
        protected T _component;


        public override void OnStart()
        {
            GameObject currentGameObject = _targetGameObject.Value != null || _targetCanBeNull.Value ? _targetGameObject.Value : gameObject;
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _component = currentGameObject != null ? currentGameObject.GetComponent<T>() : null;
            }
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _targetCanBeNull = null;
        }
    }
}