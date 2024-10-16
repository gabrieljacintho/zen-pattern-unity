using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FireRingStudio.LevelManagement
{
    public class SceneLevelManager : LevelManagerGeneric<SceneLevel>
    {
        [SerializeField] private LoadSceneMode _loadMode = LoadSceneMode.Single;
        [SerializeField] private bool _asyncLoad;

        private AsyncOperation _unloadAsyncOperation;
        private AsyncOperation _loadAsyncOperation;
        private Coroutine _loadLevelCoroutine;

        public AsyncOperation LoadAsyncOperation => _loadAsyncOperation;


        public override void LoadLevel(Level level)
        {
            if (_loadLevelCoroutine != null)
            {
                StopCoroutine(_loadLevelCoroutine);
            }

            _loadLevelCoroutine = StartCoroutine(LoadLevelRoutine(level as SceneLevel));
        }

        private IEnumerator LoadLevelRoutine(SceneLevel level)
        {
            if (_loadMode == LoadSceneMode.Additive && !string.IsNullOrEmpty(_lastLevelId))
            {
                if (FindLevelWithId(_lastLevelId) is SceneLevel lastLevel)
                {
                    _unloadAsyncOperation = lastLevel.UnloadAsync();
                    _lastLevelId = null;
                }
            }

            if (_unloadAsyncOperation != null)
            {
                yield return _unloadAsyncOperation;
            }

            if (_asyncLoad)
            {
                _loadAsyncOperation = level.LoadAsync(_loadMode);
            }
            else
            {
                level.Load(_loadMode);
            }

            _lastLevelId = level.Id;
            _loadLevelCoroutine = null;

            Save();

            OnLevelLoad?.Invoke(level);
        }
    }
}