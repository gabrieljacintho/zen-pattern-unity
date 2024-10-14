using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FireRingStudio
{
    [Serializable]
    public enum SceneSelectionMode
    {
        Name,
        Index,
        Next,
        Previous
    }

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneSelectionMode _selectionMode;
        [ShowIf("@_selectionMode == SceneSelectionMode.Name")]
        [SerializeField] private string _sceneName;
        [ShowIf("@_selectionMode == SceneSelectionMode.Index")]
        [SerializeField] private int _sceneIndex;
        [SerializeField] private LoadSceneMode _mode = LoadSceneMode.Single;
        [SerializeField] private float _minAsyncLoadSeconds;
        [SerializeField] private bool _loadOnStart;
        [ShowIf("_loadOnStart")]
        [SerializeField] private bool _async = true;

        private float _asyncLoadStartTime;
        
        public AsyncOperation AsyncOperation { get; private set; }

        public float AsyncLoadProgress => GetAsyncLoadProgress();

        [Space]
        public UnityEvent OnLoad;
        
        
        private void Start()
        {
            if (_loadOnStart)
            {
                if (_async)
                {
                    LoadAsync();
                }
                else
                {
                    Load();
                }
            }
        }

        [Button]
        public void Load()
        {
            SceneManager.LoadScene(_sceneName);
            
            OnLoad?.Invoke();
        }

        [Button]
        public void LoadAsync()
        {
            StartCoroutine(LoadAsyncRoutine());
            
            OnLoad?.Invoke();
        }

        private IEnumerator LoadAsyncRoutine()
        {
            switch (_selectionMode)
            {
                case SceneSelectionMode.Name:
                    AsyncOperation = SceneManager.LoadSceneAsync(_sceneName, _mode);
                    break;

                case SceneSelectionMode.Index:
                    AsyncOperation = SceneManager.LoadSceneAsync(_sceneIndex, _mode);
                    break;

                case SceneSelectionMode.Next:
                    int index = SceneManager.GetActiveScene().buildIndex;
                    index = Mathf.Clamp(++index, 0, SceneManager.sceneCountInBuildSettings - 1);

                    AsyncOperation = SceneManager.LoadSceneAsync(index, _mode);
                    break;

                case SceneSelectionMode.Previous:
                    index = SceneManager.GetActiveScene().buildIndex;
                    index = Mathf.Clamp(--index, 0, SceneManager.sceneCountInBuildSettings - 1);

                    AsyncOperation = SceneManager.LoadSceneAsync(index, _mode);
                    break;
            }

            _asyncLoadStartTime = Time.time;
            
            if (_minAsyncLoadSeconds > 0f)
            {
                AsyncOperation.allowSceneActivation = false;

                yield return new WaitForSeconds(_minAsyncLoadSeconds);

                AsyncOperation.allowSceneActivation = true;
            }
        }

        private float GetAsyncLoadProgress()
        {
            if (AsyncOperation == null)
            {
                return 0f;
            }

            float elapsedSeconds = Time.time - _asyncLoadStartTime;
            float minSecondsProgress = elapsedSeconds / _minAsyncLoadSeconds;
            
            float loadProgress = AsyncOperation.progress / 0.9f;
            
            return Mathf.Min(minSecondsProgress, loadProgress);
        }
    }
}