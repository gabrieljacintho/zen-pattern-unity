using System;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.UI.Settings
{
    public class SettingsHandler : MonoBehaviour
    {
        [Tooltip("If null the component GameObject is used.")]
        [SerializeField] private GameObject _settingsRoot;
        [SerializeField] private bool _loadAppliedSettingsOnEnable = true;

        private List<ISettingsHandler> _settingsHandlers;

        [Space]
        public UnityEvent OnHasChanges;
        public UnityEvent OnNotHasChanges;
        public UnityEvent OnApplyChanges;
        public UnityEvent OnCancelChanges;
        public UnityEvent OnResetSettings;


        private void Awake()
        {
            LoadSettings();
        }

        private void OnEnable()
        {
            if (_loadAppliedSettingsOnEnable)
            {
                LoadAppliedSettings();
            }
        }

        [Button]
        public void ApplyChanges()
        {
            if (_settingsHandlers == null)
            {
                return;
            }

            foreach (ISettingsHandler settingsHandler in _settingsHandlers)
            {
                if (!settingsHandler.IsApplied())
                {
                    settingsHandler.Apply();
                }
            }
            
            OnApplyChanges?.Invoke();
        }

        [Button]
        public void CancelChanges()
        {
            LoadAppliedSettings();
            
            OnCancelChanges?.Invoke();
        }

        [Button]
        public void ResetSettings()
        {
            if (_settingsHandlers == null)
            {
                return;
            }
            
            foreach (ISettingsHandler settingsHandler in _settingsHandlers)
            {
                settingsHandler.ResetSettings();
            }
            
            OnResetSettings?.Invoke();
        }

        public void LoadAppliedSettings()
        {
            if (_settingsHandlers == null)
            {
                return;
            }

            foreach (ISettingsHandler settingsHandler in _settingsHandlers)
            {
                if (settingsHandler.HasChanged())
                {
                    settingsHandler.Cancel();
                }
            }
        }

        public void CheckChanges()
        {
            if (HasChanges())
            {
                OnHasChanges?.Invoke();
            }
            else
            {
                OnNotHasChanges?.Invoke();
            }
        }
        
        public bool IsAllDefault()
        {
            return _settingsHandlers != null && !_settingsHandlers.Exists(settingsHandler => !settingsHandler.IsDefault());
        }
        
        public bool HasChanges()
        {
            return _settingsHandlers != null && _settingsHandlers.Exists(settingsHandler => settingsHandler.HasChanged());
        }

        private void LoadSettings()
        {
            GameObject root = _settingsRoot != null ? _settingsRoot : gameObject;
            _settingsHandlers = root.GetInterfacesInChildren<ISettingsHandler>(true).ToList();

            SettingsHandler[] handlers = root.GetComponentsInChildren<SettingsHandler>(true);

            List<ISettingsHandler> settings = new List<ISettingsHandler>(_settingsHandlers);
            foreach (ISettingsHandler setting in settings)
            {
                foreach (SettingsHandler handler in handlers)
                {
                    if (handler == this)
                    {
                        continue;
                    }

                    if (setting.GameObject.transform.IsChildOf(handler.transform))
                    {
                        _settingsHandlers.Remove(setting);
                        continue;
                    }
                }
            }
        }
    }
}