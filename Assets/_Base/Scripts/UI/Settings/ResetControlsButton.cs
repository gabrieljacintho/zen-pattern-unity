using FireRingStudio.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FireRingStudio.UI.Settings
{
    [RequireComponent(typeof(Button))]
    public class ResetControlsButton : ResetSettingsButton
    {
        private bool _bindingsChanged;


        protected void OnEnable()
        {
            BindingsManager.BindingsUpdated += OnBindingsUpdated;
            OnBindingsUpdated();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            BindingsManager.BindingsUpdated -= OnBindingsUpdated;
        }

        protected override bool IsInteractable()
        {
            return base.IsInteractable() || _bindingsChanged;
        }

        protected override void OnClick()
        {
            base.OnClick();
            BindingsManager.Instance.ResetBindings();
        }

        private void OnBindingsUpdated()
        {
            PlayerInput playerInput = BindingsManager.Instance != null ? BindingsManager.Instance.PlayerInput : null;
            string rebinds = playerInput != null ? playerInput.actions.SaveBindingOverridesAsJson() : null;

            _bindingsChanged = !string.IsNullOrEmpty(rebinds);
        }
    }
}