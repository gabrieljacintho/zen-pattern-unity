using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomValueList&lt;FireRingStudio.Vitals.Damage, DamageEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Damage", fileName = "DamageValueList")]
    public sealed class DamageValueList : AtomValueList<FireRingStudio.Vitals.Damage, DamageEvent> { }
}
