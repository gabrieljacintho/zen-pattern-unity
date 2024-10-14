using System.Collections.Generic;
using System.Linq;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Input
{
    public class CancelInput : InputComponent
    {
        [Tooltip("If null the component Transform is used.")]
        [SerializeField] private Transform _cancelsRoot;

        private List<ICancelable> _cancelables;

        private Transform CancelsRoot => _cancelsRoot != null ? _cancelsRoot : transform;
        
        [Space]
        public UnityEvent OnCancel;
        public UnityEvent OnNotCancel;


        protected override void Awake()
        {
            base.Awake();
            
            _cancelables = CancelsRoot.gameObject.GetInterfacesInChildren<ICancelable>(true).ToList();
            _cancelables = _cancelables.OrderByDescending(cancel => cancel.Priority).ToList();
        }

        protected override void OnPerformFunc()
        {
            foreach (ICancelable cancelable in _cancelables)
            {
                if (cancelable.CanCancel())
                {
                    cancelable.Cancel();
                    OnCancel?.Invoke();
                    return;
                }
            }
            
            OnNotCancel?.Invoke();
        }
    }
}