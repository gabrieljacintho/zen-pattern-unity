using UnityEngine;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `TMPro.TMP_FontAsset`. Inherits from `AtomEventReferenceListener&lt;TMPro.TMP_FontAsset, TMP_FontAssetEvent, TMP_FontAssetEventReference, TMP_FontAssetUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/TMP_FontAsset Event Reference Listener")]
    public sealed class TMP_FontAssetEventReferenceListener : AtomEventReferenceListener<
        TMPro.TMP_FontAsset,
        TMP_FontAssetEvent,
        TMP_FontAssetEventReference,
        TMP_FontAssetUnityEvent>
    { }
}
