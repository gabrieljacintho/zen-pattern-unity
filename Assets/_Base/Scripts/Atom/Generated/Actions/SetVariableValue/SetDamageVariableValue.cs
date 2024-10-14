using UnityEngine;
using UnityAtoms.BaseAtoms;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `FireRingStudio.Vitals.Damage`. Inherits from `SetVariableValue&lt;FireRingStudio.Vitals.Damage, DamagePair, DamageVariable, DamageConstant, DamageReference, DamageEvent, DamagePairEvent, DamageVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Damage", fileName = "SetDamageVariableValue")]
    public sealed class SetDamageVariableValue : SetVariableValue<
        FireRingStudio.Vitals.Damage,
        DamagePair,
        DamageVariable,
        DamageConstant,
        DamageReference,
        DamageEvent,
        DamagePairEvent,
        DamageDamageFunction,
        DamageVariableInstancer>
    { }
}
