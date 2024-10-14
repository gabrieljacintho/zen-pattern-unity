using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomEvent&lt;FireRingStudio.Vitals.Damage&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Damage", fileName = "DamageEvent")]
    public sealed class DamageEvent : AtomEvent<FireRingStudio.Vitals.Damage>
    {
    }
}
