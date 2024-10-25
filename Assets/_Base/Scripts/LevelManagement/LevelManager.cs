using FireRingStudio.Patterns;
using FireRingStudio.SaveSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.LevelManagement
{
    public abstract class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] protected bool _loop;
        [SerializeField] protected bool _loadNextLevelOnComplete;
        [SerializeField] protected bool _loadLastLevelOnAwake;
        [SerializeField] protected string _saveKey = "level";

        protected string _lastLevelId;

        public abstract List<Level> Levels { get; }

        [Space]
        public UnityEvent<Level> OnLevelLoad;
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

        public void LoadLastLevel()
        {
            Level level = GetLastLevel();
            LoadLevel(level);
        }

        [Button]
        public void LoadNextLevel()
        {
            Level activeLevel = FindActiveLevel();
            if (activeLevel == null || (!_loop && activeLevel == Levels[^1]))
            {
                return;
            }

            int index = Levels.IndexOf(activeLevel) + 1;
            index %= Levels.Count;

            LoadLevel(index);
        }

        #region Load Level

        public void LoadLevel(int index)
        {
            Level level = GetLevelAt(index);
            LoadLevel(level);
        }

        public void LoadLevel(string id)
        {
            Level level = FindLevelWithId(id);
            LoadLevel(level);
        }

        public virtual void LoadLevel(Level level)
        {
            level.Load();
            _lastLevelId = level.Id;
            Save();

            OnLevelLoad?.Invoke(level);
        }

        #endregion

        [Button]
        public void CompleteActiveLevel()
        {
            Level level = FindActiveLevel();
            level.Complete();

            OnLevelComplete?.Invoke(level);

            if (level == Levels[^1])
            {
                OnAllLevelsComplete?.Invoke();
            }

            if (_loadNextLevelOnComplete)
            {
                LoadNextLevel();
            }
        }

        public void ResetAllLevels()
        {
            foreach (Level level in Levels)
            {
                level.ResetLevel();
            }
        }

        #region Level Query Functions

        public Level FindActiveLevel(Predicate<Level> match = default)
        {
            Level activeLevel = Levels.Find(x => x.Id == _lastLevelId && (match == null || match(x)));
            if (activeLevel == null)
            {
                activeLevel = Levels.Find(x => x.IsLoaded() && (match == null || match(x)));
            }

            return activeLevel;
        }

        public Level FindCurrentLevel()
        {
            Level level = Levels.Find(x => !x.Completed);
            if (level == null)
            {
                level = Levels[^1];
            }

            return level;
        }

        public Level FindLevelWithId(string id)
        {
            return Levels.Find(x => x.Id == id);
        }

        public Level GetLastLevel()
        {
            if (!string.IsNullOrEmpty(_lastLevelId))
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
                index %= Levels.Count;
            }
            else
            {
                index = Mathf.Clamp(index, 0, Levels.Count - 1);
            }

            return Levels[index];
        }

        public int GetActiveLevelIndex()
        {
            int levelIndex = 0;
            Level level = FindActiveLevel();
            if (level != null)
            {
                levelIndex = Levels.IndexOf(level);
            }

            return levelIndex;
        }

        #endregion

        public void SetLastLevel(int index)
        {
            _lastLevelId = Levels[index].Id;
            Save();
        }

        public bool IsUnlocked(Level level)
        {
            Level currentLevel = FindCurrentLevel();
            int currentIndex = Levels.IndexOf(currentLevel);
            int targetIndex = Levels.IndexOf(level);

            return targetIndex <= currentIndex;
        }

        public bool AreAllLevelsCompleted()
        {
            return !Levels.Exists(x => !x.Completed);
        }

        #region Load/Save

        protected void Load()
        {
            _lastLevelId = SaveManager.GetString(_saveKey);

            foreach (Level level in Levels)
            {
                level.LoadSave();
            }
        }

        protected void Save()
        {
            SaveManager.SetString(_saveKey, _lastLevelId);
        }

        #endregion
    }
}