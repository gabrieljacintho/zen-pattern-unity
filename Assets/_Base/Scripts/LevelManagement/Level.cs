using FireRingStudio.SaveSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FireRingStudio.LevelManagement
{
    [Serializable]
    public class Level
    {
        public string Id;
        public string SceneName;

        [ShowInInspector, ReadOnly] private bool _completed;
        private bool _loaded;

        public bool Completed
        {
            get
            {
                if (!_loaded)
                {
                    LoadSave();
                }

                return _completed;
            }
        }


        public void Load(LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(SceneName, mode);
        }

        public AsyncOperation LoadAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(SceneName, mode);
        }

        public void Complete()
        {
            _completed = true;
            Save();
            Debug.Log("Level completed: \"" + SceneName + "\"");
        }

        public void ResetLevel()
        {
            _completed = false;
            _loaded = true;
            SaveManager.DeleteKey(Id);
        }

        public void LoadSave()
        {
            _completed = SaveManager.GetBool(Id);
            _loaded = true;
        }

        public void Save()
        {
            SaveManager.SetBool(Id, _completed);
        }
    }
}