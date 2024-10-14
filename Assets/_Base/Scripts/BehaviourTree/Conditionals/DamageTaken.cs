using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Vitals;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/Vitals")]
    [TaskDescription("Did the GameObject receive damage? Returns success if yes.")]
    public class DamageTaken : ConditionalComponent<Health>
    {
        [SerializeField] private SharedFloat _lastHealth;


        public override void OnStart()
        {
            base.OnStart();

            if (_component != null && _component.CurrentHealth > _lastHealth.Value)
            {
                _lastHealth.Value = _component.CurrentHealth;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_component == null || _component.CurrentHealth >= _lastHealth.Value)
            {
                return TaskStatus.Failure;
            }

            _lastHealth.Value = _component.CurrentHealth;

            return TaskStatus.Success;
        }
    }
}