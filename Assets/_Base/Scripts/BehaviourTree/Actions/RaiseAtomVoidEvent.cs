using BehaviorDesigner.Runtime.Tasks;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("Atom/Void")]
    [TaskDescription("Raise the Atom VoidEvent. Returns success.")]
    public class RaiseAtomVoidEvent : Action
    {
        [SerializeField] private VoidEvent _voidEvent;


        public override TaskStatus OnUpdate()
        {
            if (_voidEvent == null)
            {
                Debug.LogNull(nameof(VoidEvent));
                return TaskStatus.Failure;
            }

            _voidEvent.Raise();

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            _voidEvent = null;
        }
    }
}
