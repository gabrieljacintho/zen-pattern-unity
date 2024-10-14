using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomEventReferenceListener&lt;FireRingStudio.Vitals.Damage, DamageEvent, DamageEventReference, DamageUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Damage Event Reference Listener")]
    public sealed class DamageEventReferenceListener : AtomEventReferenceListener<
        FireRingStudio.Vitals.Damage,
        DamageEvent,
        DamageEventReference,
        DamageUnityEvent>
    { }
}
