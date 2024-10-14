using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/GameObject")]
    [TaskDescription("Finds a GameObject by ID. Returns success if found.")]
    public class FindWithID : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The ID of the GameObject to find")]
        [SerializeField] private SharedString _id;
        [SerializeField] private SharedBool _includeInactive;
        [SerializeField, RequiredField] private SharedGameObject _storeValue;

        
        public override TaskStatus OnUpdate()
        {
            if (_storeValue.Value == null)
            {
                _storeValue.Value = GameObjectID.FindGameObjectWithID(_id.Value, _includeInactive.Value);
            }

            return _storeValue.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _id.Value = null;
            _includeInactive.Value = false;
            _storeValue.Value = null;
        }
    }
}