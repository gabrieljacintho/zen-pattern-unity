using UnityEngine;
using System;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomVariable&lt;FireRingStudio.Vitals.Damage, DamagePair, DamageEvent, DamagePairEvent, DamageDamageFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Damage", fileName = "DamageVariable")]
    public sealed class DamageVariable : AtomVariable<FireRingStudio.Vitals.Damage, DamagePair, DamageEvent, DamagePairEvent, DamageDamageFunction>
    {
        protected override bool ValueEquals(FireRingStudio.Vitals.Damage other)
        {
            throw new NotImplementedException();
        }
    }
}
