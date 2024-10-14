using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Input
{
    public class SequenceInputEvents : InputComponent
    {
        [SerializeField] private bool _resetOnGameStart;
        
        private int _currentIndex;
        
        [Space]
        public List<UnityEvent> Events;


        protected override void OnEnable()
        {
            base.OnEnable();

            if (_resetOnGameStart)
            {
                GameManager.GameStarted += ResetEvents;
            }
        }

        protected void OnDestroy()
        {
            if (_resetOnGameStart)
            {
                GameManager.GameStarted -= ResetEvents;
            }
        }

        protected override void OnPerformFunc()
        {
            if (Events == null || Events.Count >= _currentIndex)
            {
                return;
            }
            
            Events[_currentIndex]?.Invoke();

            _currentIndex++;
        }

        private void ResetEvents()
        {
            _currentIndex = 0;
        }
    }
}