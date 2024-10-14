using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Atom
{
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Conditions/Float/InsideClamp", fileName = "FloatInsideClamp")]
    public class FloatInsideClamp : FloatCondition
    {
        [SerializeField] private ClampFloat _clamp;
        
        
        public override bool Call(float value)
        {
            return _clamp != null && value >= _clamp.Min && value <= _clamp.Max;
        }
    }
}
