using System;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `TMP_FontAssetPair`. Inherits from `AtomEventReference&lt;TMP_FontAssetPair, TMP_FontAssetVariable, TMP_FontAssetPairEvent, TMP_FontAssetVariableInstancer, TMP_FontAssetPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class TMP_FontAssetPairEventReference : AtomEventReference<
        TMP_FontAssetPair,
        TMP_FontAssetVariable,
        TMP_FontAssetPairEvent,
        TMP_FontAssetVariableInstancer,
        TMP_FontAssetPairEventInstancer>, IGetEvent 
    { }
}
