using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class CheckEvent : MonoBehaviour
    {
        [SerializeField] private List<string> _ids;
        [SerializeField] private bool _useSave;
        [ShowIf("_useSave")]
        [SerializeField] private string _saveKey = "CheckEvent";

        private Dictionary<string, bool> _checkById = new Dictionary<string, bool>();
        
        [Space]
        public UnityEvent OnCheck;
        public UnityEvent OnCheckAll;
        
        
        private void OnEnable()
        {
            if (_useSave)
            {
                Load();
            }
        }

        [Button]
        public void Check(string id)
        {
            if (_ids == null || !_ids.Contains(id))
            {
                Debug.LogError("Id not found! (\"" + id + "\")", this);
                return;
            }

            if (_checkById[id])
            {
                return;
            }
            
            _checkById[id] = true;
            
            OnCheck?.Invoke();

            if (IsAllChecked())
            {
                OnCheckAll?.Invoke();
            }
            
            if (_useSave)
            {
                Save(id);
            }
        }

        private void Load()
        {
            if (_ids == null)
            {
                return;
            }

            bool check = false;
            bool checkAll = true;
            foreach (string id in _ids)
            {
                string saveKey = GetSaveKey(id);
                
                bool canCheck = saveKey != _saveKey && PlayerPrefs.GetInt(saveKey) == 1;
                _checkById[id] = canCheck;

                if (canCheck)
                {
                    check = true;
                }
                else
                {
                    checkAll = false;
                }
            }

            if (!check)
            {
                return;
            }
            
            OnCheck?.Invoke();

            if (checkAll)
            {
                OnCheckAll?.Invoke();
            }
        }
        
        private void Save(string id)
        {
            string saveKey = GetSaveKey(id);
            if (saveKey == _saveKey)
            {
                return;
            }
            
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();
        }
        
        private string GetSaveKey(string id)
        {
            return _saveKey + (!string.IsNullOrEmpty(id) ? "_" + id : null);
        }

        private bool IsAllChecked()
        {
            if (_ids == null)
            {
                return true;
            }

            foreach (string id in _ids)
            {
                if (!_checkById[id])
                {
                    return false;
                }
            }

            return true;
        }
    }
}