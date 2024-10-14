using System;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomEventReference&lt;FireRingStudio.Vitals.Damage, DamageVariable, DamageEvent, DamageVariableInstancer, DamageEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DamageEventReference : AtomEventReference<
        FireRingStudio.Vitals.Damage,
        DamageVariable,
        DamageEvent,
        DamageVariableInstancer,
        DamageEventInstancer>, IGetEvent 
    { }
}
