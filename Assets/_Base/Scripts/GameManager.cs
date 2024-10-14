using System;
using FireRingStudio.Patterns;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio
{
    [Serializable]
    public enum GameState
    {
        NotStarted,
        InGame,
        Interaction,
        Paused,
        Over,
        Win
    }
    
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private GameState _initialState;

        private static GameState s_currentGameState = GameState.InGame;
        private static GameState s_lastGameState;

        public static GameState CurrentGameState
        {
            get => s_currentGameState;
            set => ChangeGameState(value);
        }
        public static bool InGame => s_currentGameState == GameState.InGame;
        public static bool InAnyGameState => s_currentGameState == GameState.InGame || s_currentGameState == GameState.Interaction;
        public static bool IsPaused
        {
            get => s_currentGameState == GameState.Paused;
            set => SetPaused(value);
        }
        public static float GameStartTime { get; private set; }

        public delegate void GameStateChangedEvent(GameState gameState);
        public static event GameStateChangedEvent GameStateChanged;

        public delegate void GameStateEvent();
        public static event GameStateEvent GameStarted;
        public static event GameStateEvent GameOver;
        public static event GameStateEvent GameWin;
        public static event GameStateEvent CinematicStarted;
        public static event GameStateEvent CinematicEnded;
        public static event GameStateEvent GamePaused;
        public static event GameStateEvent GameUnpaused;
        public static event GameStateEvent GameReset;

        [Header("Raises")]
        public VoidEvent StartGame;
        public VoidEvent ContinueGame;
        public VoidEvent StartCinematic;
        public VoidEvent PauseGame;
        public VoidEvent UnpauseGame;
        public VoidEvent TogglePaused;
        public VoidEvent LoseGame;
        public VoidEvent WinGame;
        public VoidEvent ResetGame;

        [Header("Listeners")]
        public VoidEvent OnGameStateChanged;
        public VoidEvent OnGameStarted;
        public VoidEvent OnCinematicStarted;
        public VoidEvent OnCinematicEnded;
        public VoidEvent OnGamePaused;
        public VoidEvent OnGameUnpaused;
        public VoidEvent OnGameOver;
        public VoidEvent OnGameWin;
        public VoidEvent OnGameReset;
        public VoidEvent OnGameOverOrReset;


        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                return;
            }

            StartGame.Register(RestartGame);
            ContinueGame.Register(() => ChangeGameState(GameState.InGame));
            LoseGame.Register(() => ChangeGameState(GameState.Over));
            WinGame.Register(() => ChangeGameState(GameState.Win));
            StartCinematic.Register(() => ChangeGameState(GameState.Interaction));
            PauseGame.Register(() => SetPaused(true));
            UnpauseGame.Register(() => SetPaused(false));
            TogglePaused.Register(TogglePause);
            ResetGame.Register(() => ChangeGameState(GameState.NotStarted));

            GameStateChanged += _ => OnGameStateChanged.Raise();
            GameStarted += OnGameStarted.Raise;
            GameOver += OnGameOver.Raise;
            GameOver += OnGameOverOrReset.Raise;
            GameWin += OnGameWin.Raise;
            CinematicStarted += OnCinematicStarted.Raise;
            CinematicEnded += OnCinematicEnded.Raise;
            GamePaused += OnGamePaused.Raise;
            GameUnpaused += OnGameUnpaused.Raise;
            GameReset += OnGameReset.Raise;
            GameReset += OnGameOverOrReset.Raise;

            CurrentGameState = _initialState;
        }

        public static void SetPaused(bool value)
        {
            if (value)
            {
                ChangeGameState(GameState.Paused);
            }
            else if (s_currentGameState == GameState.Paused)
            {
                ChangeGameState(s_lastGameState);
            }
        }
        
        public static void TogglePause() => IsPaused = !IsPaused;

        public static void RestartGame()
        {
            switch (s_currentGameState)
            {
                case GameState.Interaction:
                    CinematicEnded?.Invoke();
                    break;

                case GameState.Paused:
                    GameUnpaused?.Invoke();
                    break;
            }

            s_currentGameState = GameState.NotStarted;
            ChangeGameState(GameState.InGame);
        }

        public static void ChangeGameState(GameState newGameState)
        {
            if (s_currentGameState == newGameState)
            {
                return;
            }

            s_lastGameState = s_currentGameState;
            s_currentGameState = newGameState;

            Time.timeScale = s_currentGameState != GameState.Paused ? 1f : 0f;

            switch (s_lastGameState)
            {
                case GameState.Interaction:
                    CinematicEnded?.Invoke();
                    break;

                case GameState.Paused:
                    GameUnpaused?.Invoke();
                    break;
            }

            switch (s_currentGameState)
            {
                case GameState.NotStarted:
                    GameReset?.Invoke();
                    break;

                case GameState.InGame:
                    if (s_lastGameState == GameState.NotStarted)
                    {
                        GameStartTime = Time.time;
                        GameStarted?.Invoke();
                    }
                    break;

                case GameState.Interaction:
                    CinematicStarted?.Invoke();
                    break;

                case GameState.Paused:
                    GamePaused?.Invoke();
                    break;

                case GameState.Over:
                    GameOver?.Invoke();
                    break;

                case GameState.Win:
                    GameWin?.Invoke();
                    break;
            }

            GameStateChanged?.Invoke(s_currentGameState);

            Debug.Log("Game state changed: \"" + s_currentGameState + "\"");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void ResetTimeScale()
        {
            Time.timeScale = 1f;
        }
    }
}
