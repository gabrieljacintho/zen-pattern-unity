using UnityEngine;
using UnityAtoms.BaseAtoms;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `TMPro.TMP_FontAsset`. Inherits from `AtomVariableInstancer&lt;TMP_FontAssetVariable, TMP_FontAssetPair, TMPro.TMP_FontAsset, TMP_FontAssetEvent, TMP_FontAssetPairEvent, TMP_FontAssetTMP_FontAssetFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/TMP_FontAsset Variable Instancer")]
    public class TMP_FontAssetVariableInstancer : AtomVariableInstancer<
        TMP_FontAssetVariable,
        TMP_FontAssetPair,
        TMPro.TMP_FontAsset,
        TMP_FontAssetEvent,
        TMP_FontAssetPairEvent,
        TMP_FontAssetTMP_FontAssetFunction>
    { }
}
