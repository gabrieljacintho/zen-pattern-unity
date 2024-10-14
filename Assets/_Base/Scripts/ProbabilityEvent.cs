using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class ProbabilityEvent : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _probability = 1f;

        [Space]
        public UnityEvent OnInvoke;
        public UnityEvent OnNotInvoke;


        public void TryInvoke()
        {
            if (_probability > 0f && UnityEngine.Random.Range(0f, 1f) <= _probability)
            {
                OnInvoke?.Invoke();
            }
            else
            {
                OnNotInvoke?.Invoke();
            }
        }
    }
}