using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio.UI.Options
{
    public class SelectableOptions : ButtonComponent, ICancelable
    {
        [SerializeField] private InputActionReference _confirmInput;
        [SerializeField] private int _cancelPriority = 2;

        public int Priority => _cancelPriority;
        private List<SelectableOption> _options;
        private SelectionOptionsManager _optionsManager;

        private static float s_lastConfirmTime;

        public bool IsExecuting => Options.Exists(option => option.IsExecuting);
        public List<SelectableOption> Options
        {
            get
            {
                if (_options == null)
                {
                    _options = GetComponents<SelectableOption>().ToList();
                }

                return _options;
            }
        }
        private SelectionOptionsManager OptionsManager
        {
            get
            {
                if (_optionsManager == null)
                {
                    _optionsManager = GetComponentInParent<Canvas>().GetComponentInChildren<SelectionOptionsManager>(true);
                }

                return _optionsManager;
            }
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            
            if (_confirmInput != null)
            {
                _confirmInput.action.performed += Confirm;
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (_confirmInput != null)
            {
                _confirmInput.action.performed -= Confirm;
            }
            
            Cancel();
        }

        public void Confirm()
        {
            foreach (SelectableOption option in Options)
            {
                if (option.IsExecuting)
                {
                    option.ConfirmChanges();
                    s_lastConfirmTime = Time.time;
                }
            }
        }

        private void Confirm(InputAction.CallbackContext context)
        {
            Confirm();
        }
        
        public void Cancel()
        {
            foreach (SelectableOption option in Options)
            {
                if (option.IsExecuting)
                {
                    option.CancelChanges();
                }
            }
        }

        public bool CanCancel()
        {
            foreach (SelectableOption option in Options)
            {
                if (option.IsExecuting)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool HasAnyOptionAvailable()
        {
            foreach (SelectableOption option in Options)
            {
                if (option.IsAvailable())
                {
                    return true;
                }
            }

            return false;
        }
        
        protected override void OnClick()
        {
            if (OptionsManager != null && Time.time - s_lastConfirmTime > 0.1f)
            {
                OptionsManager.OnClickSelectableOptions(this);
            }
        }
    }
}