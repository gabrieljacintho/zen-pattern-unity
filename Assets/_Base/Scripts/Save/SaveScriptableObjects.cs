using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class SaveScriptableObjects : MonoBehaviour, ISave
    {
        [SerializeField] private string _saveKey;
        [SerializeField] private List<KeyValue<ScriptableObject, string>> _scriptableObjects;

        public string SaveKey => _saveKey;


        private void Awake()
        {
            SaveManager.RegisterSave(this);
        }

        private void OnDestroy()
        {
            SaveManager.UnregisterSave(this);
        }

        public void Load(ES3Settings settings)
        {
            if (_scriptableObjects == null || _scriptableObjects.Count == 0)
            {
                return;
            }

            foreach (KeyValue<ScriptableObject, string> scriptableObjectSaveKey in _scriptableObjects)
            {
                string saveKey = GetScriptableObjectSaveKey(scriptableObjectSaveKey);
                if (!ES3.KeyExists(saveKey))
                {
                    continue;
                }

                ES3.LoadInto(saveKey, scriptableObjectSaveKey.Key, settings);
            }
        }

        [Button]
        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void Save(ES3Settings settings)
        {
            if (_scriptableObjects == null || _scriptableObjects.Count == 0)
            {
                return;
            }

            ES3.Save(SaveKey, true, settings);

            foreach (KeyValue<ScriptableObject, string> scriptableObjectSaveKey in _scriptableObjects)
            {
                string saveKey = GetScriptableObjectSaveKey(scriptableObjectSaveKey);
                ES3.Save(saveKey, scriptableObjectSaveKey.Key, settings);
            }
        }

        [Button]
        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            if (_scriptableObjects == null || _scriptableObjects.Count == 0)
            {
                return;
            }

            ES3.DeleteKey(SaveKey, settings);

            foreach (KeyValue<ScriptableObject, string> scriptableObjectSaveKey in _scriptableObjects)
            {
                string saveKey = GetScriptableObjectSaveKey(scriptableObjectSaveKey);
                ES3.DeleteKey(saveKey, settings);
            }
        }

        [Button]
        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));

        private string GetScriptableObjectSaveKey(KeyValue<ScriptableObject, string> scriptableObjectSaveKey)
        {
            return SaveKey + "-" + scriptableObjectSaveKey.Key;
        }
    }
}