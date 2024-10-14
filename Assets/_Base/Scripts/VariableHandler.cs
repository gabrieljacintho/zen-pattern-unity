using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public abstract class VariableHandler : MonoBehaviour
    {
        [SerializeField] private IntReference _intReference;
        [SerializeField] private FloatReference _floatReference;

        [Header("Settings")]
        [SerializeField] private IntReference _initialIndex = new(0);
        [SerializeField] private int _initialIndexOffset;
        [SerializeField] private FloatReference[] _initialValueReferences = { new(1f) };
        [ShowIf("@InitialIndex > 0")]
        [SerializeField] private FloatReference _defaultValueReference = new(0f);
        [SerializeField] private FloatReference _valuePowerReference = new(1f);
        [SerializeField] private bool _limitMin;
        [ShowIf("_limitMin")]
        [SerializeField] private FloatReference _minValueReference = new(-1);
        [SerializeField] private bool _limitMax;
        [ShowIf("_limitMax")]
        [SerializeField] private FloatReference _maxValueReference = new(-1);

        protected abstract int CurrentIndex { get; }

        private int InitialIndex => (_initialIndex?.Value ?? 0) + _initialIndexOffset;
        private float DefaultValue => _defaultValueReference?.Value ?? 0;
        private float ValuePower => _valuePowerReference?.Value ?? 0;
        private float MinValue => _minValueReference?.Value ?? 0;
        private float MaxValue => _maxValueReference?.Value ?? 0;

        [Space]
        public UnityEvent<int> OnIntValueChanged;
        public UnityEvent<float> OnFloatValueChanged;


        protected virtual void OnEnable()
        {
            UpdateVariable();
        }

        public void UpdateVariable()
        {
            UpdateVariable(CurrentIndex);
        }

        protected void UpdateVariable(int index)
        {
            if (_initialValueReferences == null || _initialValueReferences.Length == 0)
            {
                Debug.LogError("There is no initial values!", this);
                return;
            }
            
            index -= InitialIndex;

            if (index < 0)
            {
                SetVariableValue(DefaultValue);
                return;
            }
            
            if (index < _initialValueReferences.Length)
            {
                float initialValue = _initialValueReferences[index]?.Value ?? 0f;
                SetVariableValue(initialValue);
                return;
            }

            float lastInitialValue = _initialValueReferences[^1]?.Value ?? 0f;
            int remainingValues = index - _initialValueReferences.Length + 1;
            float value = lastInitialValue * Mathf.Pow(ValuePower, remainingValues);
            
            SetVariableValue(value);
        }

        private void SetVariableValue(float value)
        {
            if (_limitMin)
            {
                value = Mathf.Max(value, MinValue);
            }

            if (_limitMax)
            {
                value = Mathf.Min(value, MaxValue);
            }
            
            if (_intReference != null)
            {
                int intValue = Mathf.RoundToInt(value);
                _intReference.Value = intValue;
                
                OnIntValueChanged?.Invoke(intValue);
            }
            
            if (_floatReference != null)
            {
                _floatReference.Value = value;
                
                OnFloatValueChanged?.Invoke(value);
            }
        }
    }
}