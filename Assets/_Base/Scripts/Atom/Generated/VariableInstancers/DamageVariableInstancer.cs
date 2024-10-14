using UnityEngine;
using UnityAtoms.BaseAtoms;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomVariableInstancer&lt;DamageVariable, DamagePair, FireRingStudio.Vitals.Damage, DamageEvent, DamagePairEvent, DamageDamageFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Damage Variable Instancer")]
    public class DamageVariableInstancer : AtomVariableInstancer<
        DamageVariable,
        DamagePair,
        FireRingStudio.Vitals.Damage,
        DamageEvent,
        DamagePairEvent,
        DamageDamageFunction>
    { }
}
