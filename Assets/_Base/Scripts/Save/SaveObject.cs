using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class SaveObject : MonoBehaviour, ISave
    {
        [SerializeField] private string _saveKey;
        [SerializeField] private Object _object;

        public string SaveKey => _saveKey;


        private void Awake()
        {
            SaveManager.RegisterSave(this);
        }

        private void OnDestroy()
        {
            SaveManager.UnregisterSave(this);
        }

        public void Save(ES3Settings settings)
        {
            ES3.Save(SaveKey, _object, settings);
        }

        [Button]
        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            if (ES3.KeyExists(SaveKey))
            {
                ES3.LoadInto(SaveKey, _object, settings);
            }
        }

        [Button]
        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(SaveKey, settings);
        }

        [Button]
        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));
    }
}