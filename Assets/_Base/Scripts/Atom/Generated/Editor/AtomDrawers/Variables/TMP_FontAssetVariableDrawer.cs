#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `TMPro.TMP_FontAsset`. Inherits from `AtomDrawer&lt;TMP_FontAssetVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(TMP_FontAssetVariable))]
    public class TMP_FontAssetVariableDrawer : VariableDrawer<TMP_FontAssetVariable> { }
}
#endif
