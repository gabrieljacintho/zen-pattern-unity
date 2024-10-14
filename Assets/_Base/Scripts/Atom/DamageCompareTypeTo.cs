using FireRingStudio.Vitals;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Atom
{
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Conditions/Damage/CompareTypeTo", fileName = "CompareTypeTo")]
    public class DamageCompareTypeTo : DamageCondition
    {
        [SerializeField] private DamageType _damageType;


        public override bool Call(Damage value)
        {
            return value.Type == _damageType;
        }
    }
}