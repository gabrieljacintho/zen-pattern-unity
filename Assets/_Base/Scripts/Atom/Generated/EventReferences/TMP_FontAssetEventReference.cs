using System;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `TMPro.TMP_FontAsset`. Inherits from `AtomEventReference&lt;TMPro.TMP_FontAsset, TMP_FontAssetVariable, TMP_FontAssetEvent, TMP_FontAssetVariableInstancer, TMP_FontAssetEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class TMP_FontAssetEventReference : AtomEventReference<
        TMPro.TMP_FontAsset,
        TMP_FontAssetVariable,
        TMP_FontAssetEvent,
        TMP_FontAssetVariableInstancer,
        TMP_FontAssetEventInstancer>, IGetEvent 
    { }
}
