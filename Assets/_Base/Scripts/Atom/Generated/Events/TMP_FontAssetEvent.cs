using UnityEngine;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `TMPro.TMP_FontAsset`. Inherits from `AtomEvent&lt;TMPro.TMP_FontAsset&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/TMP_FontAsset", fileName = "TMP_FontAssetEvent")]
    public sealed class TMP_FontAssetEvent : AtomEvent<TMPro.TMP_FontAsset>
    {
    }
}
