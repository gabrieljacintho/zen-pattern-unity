using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Save
{
    public abstract class SettingsSave<T> : MonoBehaviour, ISave
    {
        [SerializeField] private bool _loadOnAwake;
        [SerializeField] private bool _saveOnQuitApplication;
        
        public abstract string SaveKey { get; }
        public abstract T DefaultValue { get; }
        public abstract T CurrentValue { get; }

        [Space]
        public UnityEvent onSave;
        public UnityEvent onLoad;
        public UnityEvent onReset;

        
        protected virtual void Awake()
        {
            if (_loadOnAwake)
            {
                Load();
            }

            if (_saveOnQuitApplication)
            {
                Application.quitting += Save;
            }

            SaveManager.RegisterSave(this);
        }

        protected virtual void OnDestroy()
        {
            SaveManager.UnregisterSave(this);
        }

        public void Save(ES3Settings settings)
        {
            switch (CurrentValue)
            {
                case int currentValue:
                {
                    PlayerPrefs.SetInt(SaveKey, currentValue);
                    break;
                }
                
                case float currentValue:
                {
                    PlayerPrefs.SetFloat(SaveKey, currentValue);
                    break;
                }
                
                default:
                    Debug.LogWarning("Type not supported!");                    
                    return;
            }
            
            PlayerPrefs.Save();
            
            onSave?.Invoke();
        }

        [Button]
        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            T result;
            
            switch (DefaultValue)
            {
                case int defaultValue:
                {
                    int value = PlayerPrefs.GetInt(SaveKey, defaultValue);
                    result = (T)Convert.ChangeType(value, typeof(T));
                    break;
                }
                
                case float defaultValue:
                {
                    float value = PlayerPrefs.GetFloat(SaveKey, defaultValue);
                    result = (T)Convert.ChangeType(value, typeof(T));
                    break;
                }
                
                default:
                    Debug.LogWarning("Invalid save type!");                    
                    return;
            }
            
            Load(result);
            
            onLoad?.Invoke();
        }

        [Button]
        public void Load() => Load(new ES3Settings(ES3.Location.File));

        [Button]
        public void ResetSave()
        {
            T result;
            
            switch (DefaultValue)
            {
                case int defaultValue:
                {
                    PlayerPrefs.SetInt(SaveKey, defaultValue);
                    result = (T)Convert.ChangeType(defaultValue, typeof(T));
                    break;
                }
                
                case float defaultValue:
                {
                    PlayerPrefs.SetFloat(SaveKey, defaultValue);
                    result = (T)Convert.ChangeType(defaultValue, typeof(T));
                    break;
                }
                
                default:
                    Debug.LogWarning("Invalid save type!");                    
                    return;
            }
            
            Load(result);
            Save();
            
            onReset?.Invoke();
        }

        public void DeleteSave(ES3Settings settings) => ResetSave();

        protected abstract void Load(T value);
    }
}