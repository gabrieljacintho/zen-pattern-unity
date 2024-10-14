using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Physics
{
    public class Trigger : DetectorBase
    {
        [Space]
        [InspectorName("On Trigger Enter")] public UnityEvent<Collider> OnTriggerEnterEvent;
        [InspectorName("On Trigger Stay")] public UnityEvent<Collider> OnTriggerStayEvent;
        [InspectorName("On Trigger Exit")] public UnityEvent<Collider> OnTriggerExitEvent;


        protected virtual void OnTriggerEnter(Collider other)
        {
            if (CanDetect(other))
            {
                OnTriggerEnterEvent?.Invoke(other);
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (CanDetect(other))
            {
                OnTriggerStayEvent?.Invoke(other);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (CanDetect(other))
            {
                OnTriggerExitEvent?.Invoke(other);
            }
        }
    }
}