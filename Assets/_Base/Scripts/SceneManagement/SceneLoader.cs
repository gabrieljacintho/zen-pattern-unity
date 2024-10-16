using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FireRingStudio.SceneManagement
{
    [Serializable]
    public enum SceneSelectionMode
    {
        Name,
        Index,
        Active,
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

        public void LoadScene(string sceneName)
        {
            if (_async)
            {
                LoadAsync(sceneName);
            }
            else
            {
                Load(sceneName);
            }
        }

        public void Load()
        {
            switch (_selectionMode)
            {
                case SceneSelectionMode.Name:
                    SceneManager.LoadScene(_sceneName, _mode);
                    break;

                case SceneSelectionMode.Index:
                    SceneManager.LoadScene(_sceneIndex, _mode);
                    break;

                case SceneSelectionMode.Active:
                    int index = GetActiveSceneIndex();
                    SceneManager.LoadScene(index, _mode);
                    break;

                case SceneSelectionMode.Next:
                    index = GetNextSceneIndex();
                    SceneManager.LoadScene(index, _mode);
                    break;

                case SceneSelectionMode.Previous:
                    index = GetPreviousSceneIndex();
                    SceneManager.LoadScene(index, _mode);
                    break;
            }

            OnLoad?.Invoke();
        }

        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName, _mode);
            OnLoad?.Invoke();
        }

        public void LoadAsync()
        {
            switch (_selectionMode)
            {
                case SceneSelectionMode.Name:
                    StartCoroutine(LoadAsyncRoutine(_sceneName));
                    break;

                case SceneSelectionMode.Index:
                    StartCoroutine(LoadAsyncRoutine(_sceneIndex));
                    break;

                case SceneSelectionMode.Active:
                    int index = GetActiveSceneIndex();
                    StartCoroutine(LoadAsyncRoutine(index));
                    break;

                case SceneSelectionMode.Next:
                    index = GetNextSceneIndex();
                    StartCoroutine(LoadAsyncRoutine(index));
                    break;

                case SceneSelectionMode.Previous:
                    index = GetPreviousSceneIndex();
                    StartCoroutine(LoadAsyncRoutine(index));
                    break;
            }

            OnLoad?.Invoke();
        }

        public void LoadAsync(string sceneName)
        {
            StartCoroutine(LoadAsyncRoutine(sceneName));
            OnLoad?.Invoke();
        }

        private IEnumerator LoadAsyncRoutine(string sceneName)
        {
            AsyncOperation = SceneManager.LoadSceneAsync(sceneName, _mode);

            _asyncLoadStartTime = Time.time;

            if (_minAsyncLoadSeconds > 0f)
            {
                AsyncOperation.allowSceneActivation = false;

                yield return new WaitForSeconds(_minAsyncLoadSeconds);

                AsyncOperation.allowSceneActivation = true;
            }
        }

        private IEnumerator LoadAsyncRoutine(int sceneIndex)
        {
            AsyncOperation = SceneManager.LoadSceneAsync(sceneIndex, _mode);

            _asyncLoadStartTime = Time.time;

            if (_minAsyncLoadSeconds > 0f)
            {
                AsyncOperation.allowSceneActivation = false;

                yield return new WaitForSeconds(_minAsyncLoadSeconds);

                AsyncOperation.allowSceneActivation = true;
            }
        }

        private int GetActiveSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        private int GetNextSceneIndex()
        {
            int index = GetActiveSceneIndex();
            index = Mathf.Clamp(++index, 0, SceneManager.sceneCountInBuildSettings - 1);

            return index;
        }

        private int GetPreviousSceneIndex()
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            index = Mathf.Clamp(--index, 0, SceneManager.sceneCountInBuildSettings - 1);

            return index;
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