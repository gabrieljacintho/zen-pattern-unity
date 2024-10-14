#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `TMPro.TMP_FontAsset`. Inherits from `AtomDrawer&lt;TMP_FontAssetConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(TMP_FontAssetConstant))]
    public class TMP_FontAssetConstantDrawer : VariableDrawer<TMP_FontAssetConstant> { }
}
#endif
