using System;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Atom
{
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Conditions/Int/CompareTo", fileName = "IntCompareTo")]
    public class IntCompareTo : IntCondition
    {
        [SerializeField] private int _value;
        [SerializeField] private CompareOperator _operator;
        
        
        public override bool Call(int value)
        {
            switch (_operator)
            {
                case CompareOperator.Equal:
                    return _value == value;
                
                case CompareOperator.Different:
                    return _value != value;
                
                case CompareOperator.Greater:
                    return _value > value;
                
                case CompareOperator.GreaterOrEqual:
                    return _value >= value;
                
                case CompareOperator.Smaller:
                    return _value < value;
                
                case CompareOperator.SmallerOrEqual:
                    return _value <= value;
                
                default:
                    return false;
            }
        }
    }
}