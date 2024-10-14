using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    public abstract class SettingsSlider : SliderHandler, ISettingsHandler
    {
        [SerializeField] private bool _autoApplyChanges;

        public GameObject GameObject => gameObject;
        protected abstract float AppliedValue { get; }
        protected abstract float DefaultValue { get; }


        private void OnEnable()
        {
            CurrentValue = AppliedValue;
        }

        [Button]
        public void Apply()
        {
            Apply(CurrentValue);
        }

        [Button]
        public void Cancel() => CurrentValue = AppliedValue;

        [Button]
        public void ResetSettings() => CurrentValue = DefaultValue;

        public bool IsApplied() => Math.Abs(AppliedValue - CurrentValue) < 0.01f;
        public bool IsDefault() => Math.Abs(CurrentValue - DefaultValue) < 0.01f;

        public bool HasChanged() => Math.Abs(AppliedValue - CurrentValue) > 0.01f;

        protected override void OnValueChanged(float value)
        {
            if (_autoApplyChanges)
            {
                Apply();
            }
        }
        
        protected abstract void Apply(float value);
    }
}