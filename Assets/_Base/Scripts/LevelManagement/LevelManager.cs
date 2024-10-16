using FireRingStudio.Patterns;
using FireRingStudio.SaveSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FireRingStudio.LevelManagement
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private List<Level> _levels;
        [SerializeField] private LoadSceneMode _loadMode = LoadSceneMode.Single;
        [SerializeField] private bool _asyncLoad;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _loadNextLevelOnComplete;
        [SerializeField] private bool _loadLastLevelOnAwake;
        [SerializeField] private string _saveKey = "level";

        private Coroutine _loadLevelCoroutine;
        private AsyncOperation _unloadAsyncOperation;
        private AsyncOperation _loadAsyncOperation;
        private string _lastLevelId;
        private string _lastLevelSceneName;

        public List<Level> Levels => _levels;
        public AsyncOperation LoadAsyncOperation => _loadAsyncOperation;

        [Space]
        public UnityEvent<Level> OnLevelComplete;
        public UnityEvent OnAllLevelsComplete;


        protected override void Awake()
        {
            base.Awake();
            Load();

            if (_loadLastLevelOnAwake)
            {
                LoadLastLevel();
            }
        }

        [Button]
        public void LoadLastLevel()
        {
            Level level = GetLastLevel();
            if (level == null)
            {
                Debug.LogError("Last level not found!", this);
                return;
            }

            LoadLevel(level);
        }

        public void LoadLevel(int index)
        {
            Level level = GetLevelAt(index);
            StartCoroutine(LoadLevelRoutine(level));
        }

        public void LoadLevel(string id)
        {
            Level level = FindLevelWithId(id);
            StartCoroutine(LoadLevelRoutine(level));
        }

        public void LoadLevel(Level level)
        {
            if (_loadLevelCoroutine != null)
            {
                StopCoroutine(_loadLevelCoroutine);
            }

            _loadLevelCoroutine = StartCoroutine(LoadLevelRoutine(level));
        }

        private IEnumerator LoadLevelRoutine(Level level)
        {
            if (_loadMode == LoadSceneMode.Additive && !string.IsNullOrEmpty(_lastLevelSceneName))
            {
                _unloadAsyncOperation = SceneManager.UnloadSceneAsync(_lastLevelSceneName);
                _lastLevelSceneName = null;
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
            _lastLevelSceneName = level.SceneName;
            _loadLevelCoroutine = null;

            Save();
        }

        [Button]
        public void CompleteActiveLevel()
        {
            Level level = FindActiveLevel(x => !x.Completed);
            if (level == null)
            {
                return;
            }

            level.Complete();

            OnLevelComplete?.Invoke(level);

            if (level == _levels[^1])
            {
                OnAllLevelsComplete?.Invoke();
            }

            if (_loadNextLevelOnComplete)
            {
                LoadLastLevel();
            }
        }

        public void ResetAllLevels()
        {
            foreach (Level level in _levels)
            {
                level.ResetLevel();
            }
        }

        public Level FindCurrentLevel()
        {
            Level level = _levels.Find(x => !x.Completed);
            if (level == null)
            {
                level = _levels[^1];
            }

            return level;
        }

        public Level FindActiveLevel(Predicate<Level> match = default)
        {
            string sceneName = string.IsNullOrEmpty(_lastLevelSceneName) ? SceneManager.GetActiveScene().name : _lastLevelSceneName;
            return _levels.Find(x => x.SceneName == sceneName && (match == null || match(x)));
        }

        public Level FindLevelWithId(string id)
        {
            return _levels.Find(x => x.Id == id);
        }

        public Level GetLastLevel()
        {
            if (IsAllLevelsCompleted() && !string.IsNullOrEmpty(_lastLevelId))
            {
                return FindLevelWithId(_lastLevelId);
            }
            else
            {
                return FindCurrentLevel();
            }
        }

        public Level GetLevelAt(int index)
        {
            if (_loop)
            {
                index = Mathf.Max(index, 0);
                index %= _levels.Count;
            }
            else
            {
                index = Mathf.Clamp(index, 0, _levels.Count - 1);
            }

            return _levels[index];
        }

        public bool IsUnlocked(Level level)
        {
            Level currentLevel = FindCurrentLevel();
            int currentIndex = Levels.IndexOf(currentLevel);
            int targetIndex = Levels.IndexOf(level);

            return targetIndex <= currentIndex;
        }

        public bool IsAllLevelsCompleted()
        {
            return !_levels.Exists(x => !x.Completed);
        }

        private void Load()
        {
            _lastLevelId = SaveManager.GetString(_saveKey);

            foreach (Level level in _levels)
            {
                level.LoadSave();
            }
        }

        private void Save()
        {
            SaveManager.SetString(_saveKey, _lastLevelId);
        }
    }
}