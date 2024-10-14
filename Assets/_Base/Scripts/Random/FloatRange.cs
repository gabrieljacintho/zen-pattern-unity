using Sirenix.OdinInspector;
using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Random
{
    [Serializable]
    public struct FloatRange
    {
        public enum VariableType
        {
            Float,
            FloatVariable,
            Vector2Variable
        }

        [SerializeField, EnumToggleButtons, HideLabel] private VariableType _variableType;
        [ShowIf("@_variableType == VariableType.Float")]
        [SerializeField] private float _minValue;
        [ShowIf("@_variableType == VariableType.Float")]
        [SerializeField] private float _maxValue;
        [ShowIf("@_variableType == VariableType.FloatVariable")]
        [SerializeField] private FloatVariable _minValueVariable;
        [ShowIf("@_variableType == VariableType.FloatVariable")]
        [SerializeField] private FloatVariable _maxValueVariable;
        [ShowIf("@_variableType == VariableType.Vector2Variable")]
        [SerializeField] private Vector2Variable _valueRangeVariable;


        public readonly float GetRandomValue()
        {
            Vector2 range = GetValueRange();

            float time = UnityEngine.Random.Range(range.x, range.y);
            time = Mathf.Max(time, 0f);

            return time;
        }

        public readonly Vector2 GetValueRange()
        {
            switch (_variableType)
            {
                case VariableType.Float:
                    return new Vector2(_minValue, _maxValue);

                case VariableType.FloatVariable:
                    float minTime = _minValueVariable != null ? _minValueVariable.Value : 0f;
                    float maxTime = _maxValueVariable != null ? _maxValueVariable.Value : 0f;
                    return new Vector2(minTime, maxTime);

                case VariableType.Vector2Variable:
                    return _valueRangeVariable != null ? _valueRangeVariable.Value : Vector2.zero;

                default:
                    return Vector2.zero;
            }
        }
    }
}