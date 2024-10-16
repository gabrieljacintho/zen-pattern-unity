using FireRingStudio.SceneManagement;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.LevelManagement
{
    [Serializable]
    public enum LevelSelectionMode
    {
        Id,
        Index,
        Active,
        Current,
        Next,
        Previous
    }

    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelSelectionMode _selectionMode;
        [ShowIf("@_selectionMode == LevelSelectionMode.Id")]
        [SerializeField] private string _levelId;
        [ShowIf("@_selectionMode == LevelSelectionMode.Index")]
        [SerializeField] private int _levelIndex;
        [SerializeField] private bool _loadOnStart;

        private int _isUnlocked = -1;

        [Space]
        public UnityEvent OnLocked;
        public UnityEvent OnUnlocked;
        public UnityEvent OnLoad;


        private void Start()
        {
            if (_loadOnStart)
            {
                Load();
            }

            Level selectedLevel = GetLevel();
            Level activeLevel = LevelManager.Instance.FindActiveLevel();
            bool isUnlocked = selectedLevel != activeLevel && LevelManager.Instance.IsUnlocked(selectedLevel);

            if (_isUnlocked != 1 && isUnlocked)
            {
                _isUnlocked = 1;
                OnUnlocked?.Invoke();
            }
            else if (_isUnlocked != 0 && !isUnlocked)
            {
                _isUnlocked = 0;
                OnLocked?.Invoke();
            }
        }

        public void Load()
        {
            if (!LevelManager.HasInstance)
            {
                return;
            }

            Level level = GetLevel();
            LevelManager.Instance.LoadLevel(level);

            OnLoad?.Invoke();
        }

        private Level GetLevel()
        {
            if (!LevelManager.HasInstance)
            {
                return null;
            }

            switch (_selectionMode)
            {
                case LevelSelectionMode.Id:
                    return LevelManager.Instance.FindLevelWithId(_levelId);

                case LevelSelectionMode.Index:
                    return LevelManager.Instance.GetLevelAt(_levelIndex);

                case LevelSelectionMode.Active:
                    return LevelManager.Instance.FindActiveLevel();

                case LevelSelectionMode.Current:
                    return LevelManager.Instance.FindCurrentLevel();

                case LevelSelectionMode.Next:
                    Level level = LevelManager.Instance.FindActiveLevel();
                    int index = LevelManager.Instance.Levels.IndexOf(level);
                    return LevelManager.Instance.GetLevelAt(++index);

                case LevelSelectionMode.Previous:
                    level = LevelManager.Instance.FindActiveLevel();
                    index = LevelManager.Instance.Levels.IndexOf(level);
                    return LevelManager.Instance.GetLevelAt(--index);

                default:
                    return null;
            }
        }
    }
}