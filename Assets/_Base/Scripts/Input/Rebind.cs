using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Input
{
    public class Rebind : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputReference;
        [SerializeField] private int _bindingIndex;
        [SerializeField] private List<string> _excludedControls = new ()
        {
            "<Keyboard>/escape", "<Keyboard>/anyKey", "<Mouse>/delta", "<Mouse>/position", "<Pointer>/delta", "<Pointer>/position", "<Gamepad>/Start"
        };
        [SerializeField] private List<string> _pathRestrictions = new()
        {
            "<Keyboard>", "<Mouse>", "<Gamepad>", "<XInputController>", "<DualShockGamepad>", "<DualSenseGamepadHID>"
        };
        
        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        [Space]
        public UnityEvent OnRebindStarted;
        public UnityEvent OnRebindCompleted;
        public UnityEvent OnRebindCanceled;
        
        
        private void OnDisable()
        {
            CancelRebind();
        }

        public void StartRebind()
        {
            if (_inputReference == null)
            {
                return;
            }
            
            _rebindingOperation = _inputReference.action.PerformInteractiveRebinding()
                .WithTargetBinding(_bindingIndex)
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(RebindCompletedFunc);

            if (_excludedControls != null)
            {
                foreach (string excludedControls in _excludedControls)
                {
                    _rebindingOperation.WithControlsExcluding(excludedControls);
                }
            }

            if (_pathRestrictions != null)
            {
                foreach (string pathRestriction in _pathRestrictions)
                {
                    _rebindingOperation.WithControlsHavingToMatchPath(pathRestriction);
                }
            }

            _rebindingOperation.Start();
            
            OnRebindStarted?.Invoke();
        }
        
        public void CancelRebind()
        {
            if (_rebindingOperation == null || !_rebindingOperation.started || _rebindingOperation.completed || _rebindingOperation.canceled)
            {
                return;
            }
            
            _rebindingOperation.Cancel();
            
            OnRebindCanceled?.Invoke();
        }

        private void RebindCompletedFunc(InputActionRebindingExtensions.RebindingOperation rebindingOperation)
        {
            rebindingOperation.Dispose();
            _rebindingOperation = null;
            
            BindingsManager.BindingsUpdated?.Invoke();
            OnRebindCompleted?.Invoke();
        }
    }
}