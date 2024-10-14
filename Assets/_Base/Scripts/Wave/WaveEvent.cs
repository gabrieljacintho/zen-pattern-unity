using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Wave
{
    public class WaveEvent : MonoBehaviour
    {
        [SerializeField] private List<int> _requiredWaves;
        [SerializeField] private bool _remain;

        private int _lastWave = -1;
        
        [Space]
        public UnityEvent onWave;


        private void OnEnable()
        {
            WaveManager.WaveChanged += TryInvoke;
            TryInvoke();
        }

        private void OnDisable()
        {
            WaveManager.WaveChanged -= TryInvoke;
        }

        public void TryInvoke()
        {
            TryInvoke(WaveManager.CurrentWave);
        }

        private void TryInvoke(int wave)
        {
            if (_lastWave == wave)
            {
                return;
            }
            
            _lastWave = wave;
            
            if (_requiredWaves == null || _requiredWaves.Count == 0)
            {
                onWave?.Invoke();
                return;
            }
            
            foreach (int requiredWave in _requiredWaves)
            {
                if (requiredWave == wave)
                {
                    onWave?.Invoke();
                    return;
                }
            }

            if (_remain)
            {
                int lastRequiredWave = _requiredWaves[^1];
                if (wave > lastRequiredWave)
                {
                    onWave?.Invoke();
                }
            }
        }
    }
}