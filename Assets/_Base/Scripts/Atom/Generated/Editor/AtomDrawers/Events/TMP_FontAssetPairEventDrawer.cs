#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `TMP_FontAssetPair`. Inherits from `AtomDrawer&lt;TMP_FontAssetPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(TMP_FontAssetPairEvent))]
    public class TMP_FontAssetPairEventDrawer : AtomDrawer<TMP_FontAssetPairEvent> { }
}
#endif
