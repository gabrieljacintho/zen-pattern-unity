#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using TMPro;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `TMP_FontAssetPair`. Inherits from `AtomEventEditor&lt;TMP_FontAssetPair, TMP_FontAssetPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(TMP_FontAssetPairEvent))]
    public sealed class TMP_FontAssetPairEventEditor : AtomEventEditor<TMP_FontAssetPair, TMP_FontAssetPairEvent> { }
}
#endif
