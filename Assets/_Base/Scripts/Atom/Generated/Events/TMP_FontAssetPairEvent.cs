using UnityEngine;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `TMP_FontAssetPair`. Inherits from `AtomEvent&lt;TMP_FontAssetPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/TMP_FontAssetPair", fileName = "TMP_FontAssetPairEvent")]
    public sealed class TMP_FontAssetPairEvent : AtomEvent<TMP_FontAssetPair>
    {
    }
}
