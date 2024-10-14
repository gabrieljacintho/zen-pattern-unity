using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Animation
{
    public class MonoBehaviourFlicker : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _monoBehaviour;
        [SerializeField] private bool _canFlick = true;
        [SerializeField] private float _enableDelay = 1f;
        [SerializeField] private float _disableDelay = 1f;
        [SerializeField] private bool _onlyInGame;

        private bool _defaultEnabled;
        private float _t;
        
        public bool CanFlick
        {
            get => _canFlick;
            set => SetCanFlick(value);
        }
        private bool Enabled
        {
            get => _monoBehaviour != null && _monoBehaviour.enabled;
            set => SetEnabled(value);
        }

        [Space]
        public UnityEvent OnEnableComponent;
        public UnityEvent OnDisableComponent;


        private void Start()
        {
            _defaultEnabled = Enabled;
        }

        private void Update()
        {
            if (!_canFlick || GameManager.IsPaused || (_onlyInGame && !GameManager.InGame))
            {
                return;
            }

            _t += Time.deltaTime;
            
            if (Enabled)
            {
                if (_t >= _disableDelay)
                {
                    Enabled = false;
                }
            }
            else if (_t >= _enableDelay)
            {
                Enabled = true;
            }
        }

        public void SetCanFlick(bool value)
        {
            if (_canFlick == value)
            {
                return;
            }
            
            _canFlick = value;
            _t = 0f;

            if (!_canFlick)
            {
                Enabled = _defaultEnabled;
            }
        }

        private void SetEnabled(bool value)
        {
            if (_monoBehaviour == null || _monoBehaviour.enabled == value)
            {
                return;
            }

            _monoBehaviour.enabled = value;
            _t = 0f;

            if (_monoBehaviour.enabled)
            {
                OnEnableComponent?.Invoke();
            }
            else
            {
                OnDisableComponent?.Invoke();
            }
        }
    }
}