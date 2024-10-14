#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `TMPro.TMP_FontAsset`. Inherits from `AtomDrawer&lt;TMP_FontAssetEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(TMP_FontAssetEvent))]
    public class TMP_FontAssetEventDrawer : AtomDrawer<TMP_FontAssetEvent> { }
}
#endif
