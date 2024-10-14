using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `DamagePair`. Inherits from `AtomEventReferenceListener&lt;DamagePair, DamagePairEvent, DamagePairEventReference, DamagePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/DamagePair Event Reference Listener")]
    public sealed class DamagePairEventReferenceListener : AtomEventReferenceListener<
        DamagePair,
        DamagePairEvent,
        DamagePairEventReference,
        DamagePairUnityEvent>
    { }
}
