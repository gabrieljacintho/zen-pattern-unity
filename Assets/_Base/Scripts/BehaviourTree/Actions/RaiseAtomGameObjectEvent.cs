using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace FireRingStudio.BehaviourTree
{
    [Serializable]
    [TaskCategory("Atom/GameObject")]
    [TaskDescription("Raise the Atom GameObjectEvent. Returns success.")]
    public class RaiseAtomGameObjectEvent : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private GameObjectEvent _gameObjectEvent;

        private GameObject _lastGameObject;

        public override void OnStart()
        {
            _lastGameObject = GetDefaultGameObject(_targetGameObject.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (_gameObjectEvent == null)
            {
                Debug.LogNull(nameof(GameObjectEvent));
                return TaskStatus.Failure;
            }
            
            _gameObjectEvent.Raise(_lastGameObject);
            
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _gameObjectEvent = null;
        }
    }
}