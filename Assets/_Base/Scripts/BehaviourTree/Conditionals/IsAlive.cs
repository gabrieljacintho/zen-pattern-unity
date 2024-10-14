using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Vitals;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/Vitals")]
    [TaskDescription("Is the GameObject alive? Returns success if true.")]
    public class IsAlive : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedBool _targetCanBeNull;

        private GameObject _previousGameObject;
        private Health _health;

        
        public override void OnStart()
        {
            GameObject currentGameObject = _targetGameObject.Value;
            if (currentGameObject == null && !_targetCanBeNull.Value)
            {
                currentGameObject = gameObject;
            }

            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _health = currentGameObject != null ? currentGameObject.GetComponent<Health>() : null;
            }
        }

        public override TaskStatus OnUpdate()
        {
            return _health != null && !_health.IsDead ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
        }
    }
}