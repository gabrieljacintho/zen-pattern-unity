using FireRingStudio.Patterns;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Difficulty
{
    public class DifficultyManager : Singleton<DifficultyManager>
    {
        [SerializeField] private LocalizedString[] _levels = { "Difficulties/Easy", "Difficulties/Medium", "Difficulties/Hard", "Difficulties/VeryHard" };
        [SerializeField] private int _defaultLevel = 1;
        
        private static int s_currentLevel = 1;
        
        [Space]
        [FormerlySerializedAs("onLevelChanged")]
        public UnityEvent<int> OnLevelChanged;
        
        public delegate void DifficultyEvent(int level);
        public static event DifficultyEvent LevelChanged;
        
        public static LocalizedString[] Levels { get; private set; } = { "Difficulties/Easy", "Difficulties/Medium", "Difficulties/Hard", "Difficulties/VeryHard" };
        public static int DefaultLevel { get; private set; } = 1;

        public static int CurrentLevel
        {
            get => s_currentLevel;
            set => SetCurrentLevel(value);
        }


        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
                return;

            Levels = _levels;
            DefaultLevel = _defaultLevel;
            CurrentLevel = _defaultLevel;
        }

        private void OnEnable()
        {
            if (Instance != this)
                return;

            LevelChanged += OnLevelChanged.Invoke;
        }

        private void OnDisable()
        {
            if (Instance != this)
                return;
            
            LevelChanged -= OnLevelChanged.Invoke;
        }

        [Button]
        public static void IncreaseLevel() => CurrentLevel++;

        [Button]
        public static void DecreaseLevel() => CurrentLevel--;

        [Button]
        public static void SetCurrentLevel(int value)
        {
            if (Levels == null || value >= Levels.Length)
            {
                Debug.LogNoWithLabel("difficulty", "level", value.ToString());
                return;
            }
            
            if (s_currentLevel == value)
            {
                return;
            }
            
            s_currentLevel = value;
            LevelChanged?.Invoke(value);
            
            Debug.Log("Difficulty level changed to " + value + " (\"" + Levels[value] + "\").");
        }
    }
}