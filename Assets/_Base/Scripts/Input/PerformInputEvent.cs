using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Input
{
    public class PerformInputEvent : InputComponent
    {
        [Space]
        [FormerlySerializedAs("onPerformed")]
        public UnityEvent OnPerform;
        [SerializeField] private bool _invokeOnNextFrame;


        protected override void OnPerformFunc()
        {
            if (_invokeOnNextFrame)
            {
                this.DoOnNextFrame(OnPerform.Invoke);
            }
            else
            {
                OnPerform?.Invoke();
            }
        }
    }
}
