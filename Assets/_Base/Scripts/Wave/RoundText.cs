using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace FireRingStudio.Wave
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class RoundText : MonoBehaviour
    {
        [SerializeField] private Color _transitionColor = Color.white;

        [Space]
        [SerializeField] private bool _updateOnEnable;
        [SerializeField] private bool _updateEveryFrame;

        private TextMeshProUGUI _text;
        
        private Color _defaultColor;
        
        private static string CurrentWaveText => (WaveManager.CurrentWave + 1).ToString();

        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _defaultColor = _text.color;
        }

        private void OnEnable()
        {
            if (_updateOnEnable)
            {
                UpdateText();
            }
        }

        private void Update()
        {
            if (_updateEveryFrame)
            {
                UpdateText();
            }
        }

        [Button]
        public void UpdateText()
        {
            _text.text = CurrentWaveText;
            SetColorToDefault();
        }

        public void SetColorToTransition()
        {
            _text.color = _transitionColor;
        }

        public void SetColorToDefault()
        {
            _text.color = _defaultColor;
        }
    }
}