#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.SaveSystem
{
    public class SavedEvent : MonoBehaviour
    {
        [SerializeField] private string _saveKey = "saved-event";
        [SerializeField] private bool _autoInvoke;
        [SerializeField] private bool _autoLoad = true;
        [SerializeField] private bool _autoSave = true;
        [SerializeField] private bool _canInvokeAgain = true;
        [SerializeField] private bool _alwaysSave;

        private bool _invoked;
        private bool _loaded;

        [Space]
        public UnityEvent Event;
        public UnityEvent EventLoaded;
        public UnityEvent OnSave;


        private void Start()
        {
            if(_saveKey.Length == 0)
            {
                _saveKey = gameObject.name + transform.position + gameObject.scene.name;
            }
            if (_autoLoad && !_loaded)
            {
                Load();
            }
            if (_autoInvoke && !_invoked)
            {
                Invoke();
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Invoke()
        {
            if (_autoLoad && !_loaded)
            {
                Load();
            }

            if (_invoked && !_canInvokeAgain)
            {
                return;
            }

            Event?.Invoke();
            _invoked = true;
            if (_autoSave)
            {
                Save();
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Load()
        {
            _invoked = SaveManager.GetBool(_saveKey);
            _loaded = true;
            if (_invoked)
            {
                EventLoaded?.Invoke();
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Save()
        {
            if (!_alwaysSave)
            {
                if (!_invoked || SaveManager.GetBool(_saveKey))
                {
                    return;
                }
            }

            SaveManager.SetBool(_saveKey, _invoked);
            OnSave?.Invoke();
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void DeleteSave()
        {
            SaveManager.DeleteKey(_saveKey);
        }
    }
}