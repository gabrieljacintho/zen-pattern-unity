using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class SaveFunctions : MonoBehaviour
    {
        [Button]
        public void Save()
        {
            SaveManager.Save();
        }

        [Button]
        public void Load()
        {
            SaveManager.Load();
        }

        [Button]
        public void DeleteSave()
        {
            ES3.DeleteFile();

            Debug.Log("Game save deleted.");
        }
    }
}