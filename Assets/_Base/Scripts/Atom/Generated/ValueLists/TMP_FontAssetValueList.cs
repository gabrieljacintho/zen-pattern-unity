using UnityEngine;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `TMPro.TMP_FontAsset`. Inherits from `AtomValueList&lt;TMPro.TMP_FontAsset, TMP_FontAssetEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/TMP_FontAsset", fileName = "TMP_FontAssetValueList")]
    public sealed class TMP_FontAssetValueList : AtomValueList<TMPro.TMP_FontAsset, TMP_FontAssetEvent> { }
}
