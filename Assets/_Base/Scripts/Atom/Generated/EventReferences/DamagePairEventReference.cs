using System;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `DamagePair`. Inherits from `AtomEventReference&lt;DamagePair, DamageVariable, DamagePairEvent, DamageVariableInstancer, DamagePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DamagePairEventReference : AtomEventReference<
        DamagePair,
        DamageVariable,
        DamagePairEvent,
        DamageVariableInstancer,
        DamagePairEventInstancer>, IGetEvent 
    { }
}
