using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class GameStateEvents : MonoBehaviour
    {
        [SerializeField] private bool _invokeOnEnable;
        
        [Space]
        public UnityEvent OnNotStarted;
        public UnityEvent OnInGame;
        public UnityEvent OnCinematic;
        public UnityEvent OnPaused;
        public UnityEvent OnEnded;


        private void OnEnable()
        {
            if (_invokeOnEnable)
            {
                InvokeEvent();
            }
        }

        [Button]
        public void InvokeEvent()
        {
            switch (GameManager.CurrentGameState)
            {
                case GameState.NotStarted:
                    OnNotStarted?.Invoke();
                    break;
                
                case GameState.InGame:
                    OnInGame?.Invoke();
                    break;
                
                case GameState.Interaction:
                    OnCinematic?.Invoke();
                    break;
                
                case GameState.Paused:
                    OnPaused?.Invoke();
                    break;
                
                case GameState.Over:
                    OnEnded.Invoke();
                    break;
            }
        }
    }
}