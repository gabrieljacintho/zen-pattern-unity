using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/GameObject")]
    [TaskDescription("Finds a GameObject child with ID. Returns success if found.")]
    public class FindChildWithID : Conditional
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The ID of the GameObject to find")]
        [SerializeField] private SharedString _id;
        [SerializeField] private SharedBool _includeInactive;
        [SerializeField, RequiredField] private SharedGameObject _storeValue;

        private GameObject _previousGameObject;


        public override TaskStatus OnUpdate()
        {
            GameObject currentGameObject = GetDefaultGameObject(_targetGameObject.Value);
            if (currentGameObject != _previousGameObject)
            {
                _previousGameObject = currentGameObject;
                _storeValue.Value = currentGameObject.FindChildWithID(_id.Value, _includeInactive.Value);
            }

            return _storeValue.Value != null ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            _targetGameObject.Value = null;
            _id.Value = null;
            _includeInactive.Value = false;
            _storeValue.Value = null;
        }
    }
}