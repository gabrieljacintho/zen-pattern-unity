using FireRingStudio.Save;
using System.Collections;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    public class SaveInventoryData : MonoBehaviour, ISave
    {
        [SerializeField, ES3NonSerializable] private string _saveKey;
        [SerializeField] private InventoryData _inventoryData;

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
            ES3.Save(SaveKey, _inventoryData, settings);
        }

        public void Save() => Save(new ES3Settings(ES3.Location.File));

        public void Load(ES3Settings settings)
        {
            if (ES3.KeyExists(SaveKey))
            {
                ES3.LoadInto(SaveKey, _inventoryData, settings);
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