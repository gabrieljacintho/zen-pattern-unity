using System;
using UnityAtoms.BaseAtoms;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomReference&lt;FireRingStudio.Vitals.Damage, DamagePair, DamageConstant, DamageVariable, DamageEvent, DamagePairEvent, DamageDamageFunction, DamageVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DamageReference : AtomReference<
        FireRingStudio.Vitals.Damage,
        DamagePair,
        DamageConstant,
        DamageVariable,
        DamageEvent,
        DamagePairEvent,
        DamageDamageFunction,
        DamageVariableInstancer>, IEquatable<DamageReference>
    {
        public DamageReference() : base() { }
        public DamageReference(FireRingStudio.Vitals.Damage value) : base(value) { }
        public bool Equals(DamageReference other) { return base.Equals(other); }
        protected override bool ValueEquals(FireRingStudio.Vitals.Damage other)
        {
            throw new NotImplementedException();
        }
    }
}
