using UnityEngine;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `TMP_FontAssetPair`. Inherits from `AtomEventReferenceListener&lt;TMP_FontAssetPair, TMP_FontAssetPairEvent, TMP_FontAssetPairEventReference, TMP_FontAssetPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/TMP_FontAssetPair Event Reference Listener")]
    public sealed class TMP_FontAssetPairEventReferenceListener : AtomEventReferenceListener<
        TMP_FontAssetPair,
        TMP_FontAssetPairEvent,
        TMP_FontAssetPairEventReference,
        TMP_FontAssetPairUnityEvent>
    { }
}
