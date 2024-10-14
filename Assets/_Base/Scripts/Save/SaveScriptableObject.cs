using UnityEngine;

namespace FireRingStudio.Save
{
    public abstract class SaveScriptableObject<T> : ScriptableObject, ISave where T : SaveScriptableObject<T>
    {
        [SerializeField] private string _saveKey;

        public string SaveKey => _saveKey;


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