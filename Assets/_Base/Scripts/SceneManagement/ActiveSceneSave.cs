using FireRingStudio.SaveSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace FireRingStudio.SceneManagement
{
    public class ActiveSceneSave : MonoBehaviour
    {
        [SerializeField] private string _saveKey = "active-scene";
        [SerializeField] private string _defaultSceneName;
        [SerializeField] private bool _loadOnAwake;
        [ShowIf("_loadOnAwake")]
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private bool _saveOnAwake;


        private void Awake()
        {
            if (_loadOnAwake)
            {
                Load();
            }

            if (_saveOnAwake)
            {
                Save();
            }
        }

        public void Load()
        {
            string sceneName = SaveManager.GetString(_saveKey, _defaultSceneName);
            if (string.IsNullOrEmpty(sceneName) || _sceneLoader == null)
            {
                return;
            }

            _sceneLoader.LoadScene(sceneName);
        }

        public void Save()
        {
            Scene scene = SceneManager.GetActiveScene();
            SaveManager.SetString(_saveKey, scene.name);
        }
    }
}