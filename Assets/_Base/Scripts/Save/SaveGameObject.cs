using UnityEngine;

namespace FireRingStudio.Save
{
    public class SaveGameObject : MonoBehaviour, ISave
    {
        [SerializeField] private string _saveKey;

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
            ES3.Save(_saveKey, gameObject, settings);
        }

        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            if (ES3.KeyExists(_saveKey, settings))
            {
                ES3.LoadInto(_saveKey, gameObject, settings);
            }
        }

        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(_saveKey, settings);
        }

        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));
    }
}