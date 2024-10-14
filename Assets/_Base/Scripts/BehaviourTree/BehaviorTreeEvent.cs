using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.BehaviourTree
{
    public class BehaviorTreeEvent : MonoBehaviour
    {
        [SerializeField] private BehaviorTree _behaviorTree;
        [SerializeField] private string _eventName;

        [Space]
        public UnityEvent Event;


        private void OnEnable()
        {
            _behaviorTree.RegisterEvent(_eventName, CallEvent);
        }

        private void OnDisable()
        {
            _behaviorTree.UnregisterEvent(_eventName, CallEvent);
        }

        private void CallEvent()
        {
            Event?.Invoke();
        }
    }
}