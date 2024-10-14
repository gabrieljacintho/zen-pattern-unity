using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `DamagePair`. Inherits from `AtomEventInstancer&lt;DamagePair, DamagePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/DamagePair Event Instancer")]
    public class DamagePairEventInstancer : AtomEventInstancer<DamagePair, DamagePairEvent> { }
}
