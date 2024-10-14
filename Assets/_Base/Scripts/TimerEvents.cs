using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class TimerEvents : Timer
    {
        [Space]
        public UnityEvent OnStart;
        public UnityEvent OnStop;
        public UnityEvent OnEnd;


        protected override void OnStartTimer()
        {
            OnStart?.Invoke();
        }

        protected override void OnStopTimer()
        {
            OnStop?.Invoke();
        }

        public override void OnEndTimer()
        {
            OnEnd?.Invoke();
        }
    }
}