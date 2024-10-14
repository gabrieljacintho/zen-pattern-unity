#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using TMPro;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `TMPro.TMP_FontAsset`. Inherits from `AtomEventEditor&lt;TMPro.TMP_FontAsset, TMP_FontAssetEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(TMP_FontAssetEvent))]
    public sealed class TMP_FontAssetEventEditor : AtomEventEditor<TMPro.TMP_FontAsset, TMP_FontAssetEvent> { }
}
#endif
