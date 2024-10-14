using System.Collections;
using FireRingStudio.Random;
using FireRingStudio.Save;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public enum TimeMeasure
    {
        Seconds,
        Frames
    }

    public abstract class Timer : SaveComponent<Timer>
    {
        [SerializeField, EnumToggleButtons, HideLabel] private TimeMeasure _timeMeasures;
        [SerializeField, EnumToggleButtons, HideLabel] private FloatRange _timeRange;
        [ShowIf("@_timeMeasures == TimeMeasure.Seconds")]
        [SerializeField] private bool _useUnscaledDeltaTime;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _onlyInGame;
        [SerializeField] private bool _canRestartBeforeStop = true;

        [Header("Save")]
        [SerializeField] private bool _replayEnd = true;

        [Space]
        [SerializeField] private bool _startOnEnable;
        [SerializeField] private bool _startOnGameStart;

        public bool IsRunning { get; set; }
        public bool IsPaused { get; set; }
        public float ElapsedTime { get; set; }
        public bool Ended { get; set; }
        public bool ReplayEnd => _replayEnd;


        protected override void Awake()
        {
            base.Awake();

            if (_startOnGameStart)
            {
                GameManager.GameStarted += StartTimer;
            }
        }

        protected virtual void OnEnable()
        {
            if (_startOnEnable)
            {
                StartTimer();
            }
        }

        protected virtual void OnDisable()
        {
            if (_startOnGameStart)
            {
                GameManager.GameStarted -= StartTimer;
            }

            StopTimer();
        }

        [HideIf("IsRunning")]
        [Button]
        public void StartTimer()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (IsRunning && !_canRestartBeforeStop)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(TimerRoutine());

            OnStartTimer();
        }

        [ShowIf("IsRunning")]
        [Button]
        public void StopTimer()
        {
            StopAllCoroutines();
            IsRunning = false;
            ElapsedTime = 0f;

            OnStopTimer();
        }

        [HideIf("IsPaused")]
        [Button]
        public void PauseTimer() => IsPaused = true;

        [ShowIf("IsPaused")]
        [Button]
        public void ResumeTimer() => IsPaused = false;

        protected virtual void OnStartTimer() { }

        protected virtual void OnStopTimer() { }

        protected virtual void OnUpdateTimer() { }

        public abstract void OnEndTimer();

        public IEnumerator TimerRoutine(bool continueTime = false)
        {
            bool ranOnce = false;

            while (!ranOnce || _loop)
            {
                IsRunning = true;

                if (!continueTime || ranOnce)
                {
                    ElapsedTime = 0f;
                }

                float time = _timeRange.GetRandomValue();
                while (ElapsedTime < time)
                {
                    if (!CanRun())
                    {
                        yield return null;
                        continue;
                    }

                    ElapsedTime += GetElapsedDeltaTime();

                    OnUpdateTimer();

                    yield return null;
                }

                if (!_loop)
                {
                    IsRunning = false;
                }

                OnEndTimer();

                ranOnce = true;
                Ended = true;
            }
        }

        protected virtual bool CanRun()
        {
            if (IsPaused)
            {
                return false;
            }

            if (_onlyInGame && !GameManager.InGame)
            {
                return false;
            }

            return true;
        }

        private float GetElapsedDeltaTime()
        {
            float elapsedDeltaTime = 0f;

            switch (_timeMeasures)
            {
                case TimeMeasure.Seconds:
                    elapsedDeltaTime += _useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    break;

                case TimeMeasure.Frames:
                    elapsedDeltaTime++;
                    break;
            }

            return elapsedDeltaTime;
        }
    }
}