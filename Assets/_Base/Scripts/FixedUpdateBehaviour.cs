using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public abstract class FixedUpdateBehaviour : MonoBehaviour
    {
        [SuffixLabel("seconds", Overlay = true)]
        [Tooltip("Seconds interval between calls.")]
        [SerializeField] private float _interval = 0.02f;

        private float _startTime;
        private float _lastUpdateTime;


        protected virtual void OnDisable()
        {
            _startTime = Time.time;
        }

        protected virtual void Update()
        {
            float diffTime = Time.time - _startTime - _lastUpdateTime;
            float intervals = diffTime / _interval;
            int calls = Mathf.FloorToInt(intervals);

            for (int i = 0; i < calls; i++)
            {
                OnUpdate();
            }

            if (calls > 0)
            {
                _lastUpdateTime = Time.time - (intervals - calls);
            }
        }

        protected abstract void OnUpdate();
    }
}