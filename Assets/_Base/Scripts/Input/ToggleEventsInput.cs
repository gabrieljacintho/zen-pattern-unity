using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Input
{
    public class ToggleEventsInput : InputComponent
    {
        [SerializeField] private bool _on = true;
        [SerializeField] private bool _invokeEventOnStart = true;

        public bool On
        {
            get => _on;
            set => _on = value;
        }

        [Space]
        public UnityEvent OnTurnOn;
        public UnityEvent OnTurnOff;


        private void Start()
        {
            if (_invokeEventOnStart)
            {
                InvokeEvent();
            }
        }

        protected override void OnPerformFunc()
        {
            SetOn(!On);
        }

        public void InvokeEvent()
        {
            if (On)
            {
                OnTurnOn?.Invoke();
            }
            else
            {
                OnTurnOff?.Invoke();
            }
        }

        public void SetOn(bool value)
        {
            if (_on == value)
            {
                return;
            }

            _on = value;
            InvokeEvent();
        }
    }
}