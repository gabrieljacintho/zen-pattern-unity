using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FireRingStudio.LevelManagement
{
    [Serializable]
    public class SceneLevel : Level
    {
        public string SceneName;


        public override void Load()
        {
            Load(LoadSceneMode.Single);
        }

        public void Load(LoadSceneMode mode)
        {
            SceneManager.LoadScene(SceneName, mode);
        }

        public AsyncOperation LoadAsync(LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(SceneName, mode);
        }

        public override void Unload()
        {
            UnloadAsync();
        }

        public AsyncOperation UnloadAsync()
        {
            return SceneManager.UnloadSceneAsync(SceneName);
        }

        public override bool IsLoaded()
        {
            return SceneManager.GetSceneByName(SceneName).isLoaded;
        }
    }
}
