#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `TMPro.TMP_FontAsset`. Inherits from `AtomDrawer&lt;TMP_FontAssetValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(TMP_FontAssetValueList))]
    public class TMP_FontAssetValueListDrawer : AtomDrawer<TMP_FontAssetValueList> { }
}
#endif
