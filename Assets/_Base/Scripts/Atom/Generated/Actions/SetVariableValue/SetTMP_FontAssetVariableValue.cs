using UnityEngine;
using UnityAtoms.BaseAtoms;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `TMPro.TMP_FontAsset`. Inherits from `SetVariableValue&lt;TMPro.TMP_FontAsset, TMP_FontAssetPair, TMP_FontAssetVariable, TMP_FontAssetConstant, TMP_FontAssetReference, TMP_FontAssetEvent, TMP_FontAssetPairEvent, TMP_FontAssetVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/TMP_FontAsset", fileName = "SetTMP_FontAssetVariableValue")]
    public sealed class SetTMP_FontAssetVariableValue : SetVariableValue<
        TMPro.TMP_FontAsset,
        TMP_FontAssetPair,
        TMP_FontAssetVariable,
        TMP_FontAssetConstant,
        TMP_FontAssetReference,
        TMP_FontAssetEvent,
        TMP_FontAssetPairEvent,
        TMP_FontAssetTMP_FontAssetFunction,
        TMP_FontAssetVariableInstancer>
    { }
}
