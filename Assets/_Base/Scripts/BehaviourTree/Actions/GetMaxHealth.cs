using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Vitals;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/Vitals")]
    [TaskDescription("Gets the max health. Returns success.")]
    public class GetMaxHealth : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedFloat _percent = 1f;
        [SerializeField] private SharedFloat _storeValue;

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
                Debug.LogNull(nameof(Health));
                return TaskStatus.Failure;
            }

            _storeValue.Value = _health.MaxHealth * _percent.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _percent.Value = 1f;
            _storeValue = null;
        }
    }
}