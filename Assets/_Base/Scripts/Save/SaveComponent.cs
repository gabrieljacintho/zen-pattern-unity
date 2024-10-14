using UnityEngine;

namespace FireRingStudio.Save
{
    public abstract class SaveComponent<T> : MonoBehaviour, ISave where T : SaveComponent<T>
    {
        [SerializeField] private string _saveKey;

        public string SaveKey => _saveKey;


        protected virtual void Awake()
        {
            SaveManager.RegisterSave(this);
        }

        protected virtual void OnDestroy()
        {
            SaveManager.UnregisterSave(this);
        }

        public void Save(ES3Settings settings)
        {
            ES3.Save(SaveKey, (T)this, settings);
        }

        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            if (ES3.KeyExists(SaveKey))
            {
                ES3.LoadInto(SaveKey, (T)this, settings);
            }
        }

        public void Load() => Load(new ES3Settings(ES3.Location.File));

        public void DeleteSave(ES3Settings settings)
        {
            ES3.DeleteKey(SaveKey, settings);
        }
        
        public void DeleteSave() => DeleteSave(new ES3Settings(ES3.Location.File));
    }
}