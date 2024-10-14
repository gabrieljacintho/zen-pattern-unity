using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomEventInstancer&lt;FireRingStudio.Vitals.Damage, DamageEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Damage Event Instancer")]
    public class DamageEventInstancer : AtomEventInstancer<FireRingStudio.Vitals.Damage, DamageEvent> { }
}
