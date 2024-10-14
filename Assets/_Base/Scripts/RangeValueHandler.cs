using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public abstract class RangeValueHandler : MonoBehaviour
    {
        [SerializeField] private FloatVariable _valueVariable;
        [SerializeField] private FloatReference _maxValueReference = new(100f);
        
        [Header("Settings")]
        [SerializeField] private Vector2 _range = new Vector2(0f, 1f);
        [SerializeField] private AnimationCurve _rangeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private bool _restoreOnEnable;

        [Space]
        public UnityEvent OnMin;
        public UnityEvent OnMiddle;
        public UnityEvent OnMax;
        
        private float _velocity;
        private float _defaultRangeValue;

        private int _lastState;

        private float Value => _valueVariable != null ? _valueVariable.Value : 0f;
        private float MaxValue => _maxValueReference != null ? _maxValueReference.Value : 0f;


        protected virtual void Awake()
        {
            _defaultRangeValue = GetRangeValue();
        }

        protected virtual void OnEnable()
        {
            if (_restoreOnEnable)
            {
                Restore();
            }
        }

        protected virtual void LateUpdate()
        {
            float currentRangeValue = GetRangeValue();

            float t = Mathf.Clamp(Value / MaxValue, 0f, 1f);
            
            float targetRangeValue = _rangeCurve.Evaluate(t);
            targetRangeValue = Mathf.Lerp(_range.x, _range.y, targetRangeValue);
            
            currentRangeValue = Mathf.SmoothDamp(currentRangeValue, targetRangeValue, ref _velocity, _smoothTime);
            
            SetRangeValue(currentRangeValue);
            UpdateState(t);
        }

        public void Restore()
        {
            SetRangeValue(_defaultRangeValue);
        }

        protected abstract float GetRangeValue();

        protected abstract void SetRangeValue(float value);
        
        private void UpdateState(float t)
        {
            int currentState;
            switch (t)
            {
                case <= 0.01f:
                    currentState = 0;
                    break;
                
                case >= 0.99f:
                    currentState = 2;
                    break;
                
                default:
                    currentState = 1;
                    break;
            }

            if (_lastState == currentState)
            {
                return;
            }

            switch (currentState)
            {
                case 0:
                    OnMin?.Invoke();
                    break;
                
                case 1:
                    OnMiddle?.Invoke();
                    break;
                
                default:
                    OnMax?.Invoke();
                    break;
            }

            _lastState = currentState;
        }
    }
}