using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Vitals;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/Vitals")]
    public class HealthThreshold : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedFloat _healthPercentThreshold;

        private GameObject _previousGameObject;
        private Health _health;


        public override void OnStart()
        {
            GameObject currentGameObject = _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _health = currentGameObject.GetComponent<Health>();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_health == null)
            {
                return TaskStatus.Failure;
            }

            float t = Mathf.Clamp01(_health.CurrentHealth / _health.MaxHealth);
            if (t > _healthPercentThreshold.Value)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _healthPercentThreshold = null;
        }
    }
}