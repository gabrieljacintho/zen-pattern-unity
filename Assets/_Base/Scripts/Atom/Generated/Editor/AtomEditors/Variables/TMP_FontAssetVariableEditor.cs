using UnityEditor;
using UnityAtoms.Editor;
using TMPro;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `TMPro.TMP_FontAsset`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(TMP_FontAssetVariable))]
    public sealed class TMP_FontAssetVariableEditor : AtomVariableEditor<TMPro.TMP_FontAsset, TMP_FontAssetPair> { }
}
