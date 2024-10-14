using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Difficulty
{
    public class DifficultyEvent : MonoBehaviour
    {
        public UnityEvent OnEasy;
        public UnityEvent OnMedium;
        public UnityEvent OnHard;
        public UnityEvent OnVeryHard;
        [SerializeField] private bool _playOnEnable;


        private void OnEnable()
        {
            if (_playOnEnable)
            {
                InvokeEvent();
            }
        }

        public void InvokeEvent()
        {
            switch (DifficultyManager.CurrentLevel)
            {
                case 0:
                    OnEasy.Invoke();
                    break;

                case 1:
                    OnMedium.Invoke();
                    break;

                case 2:
                    OnHard.Invoke();
                    break;

                case 3:
                    OnVeryHard.Invoke();
                    break;
            }
        }
    }
}