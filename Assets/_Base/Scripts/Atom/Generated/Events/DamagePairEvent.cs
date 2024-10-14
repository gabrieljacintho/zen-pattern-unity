using UnityEngine;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `DamagePair`. Inherits from `AtomEvent&lt;DamagePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/DamagePair", fileName = "DamagePairEvent")]
    public sealed class DamagePairEvent : AtomEvent<DamagePair>
    {
    }
}
