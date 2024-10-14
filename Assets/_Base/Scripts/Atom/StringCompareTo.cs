using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Atom
{
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Conditions/String/CompareTo", fileName = "StringCompareTo")]
    public class StringCompareTo : StringCondition
    {
        [SerializeField] private string _value;
        
        
        public override bool Call(string value)
        {
            return value == _value;
        }
    }
}