using UnityEngine;
using System;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `TMPro.TMP_FontAsset`. Inherits from `AtomVariable&lt;TMPro.TMP_FontAsset, TMP_FontAssetPair, TMP_FontAssetEvent, TMP_FontAssetPairEvent, TMP_FontAssetTMP_FontAssetFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/TMP_FontAsset", fileName = "TMP_FontAssetVariable")]
    public sealed class TMP_FontAssetVariable : AtomVariable<TMPro.TMP_FontAsset, TMP_FontAssetPair, TMP_FontAssetEvent, TMP_FontAssetPairEvent, TMP_FontAssetTMP_FontAssetFunction>
    {
        protected override bool ValueEquals(TMPro.TMP_FontAsset other)
        {
            throw new NotImplementedException();
        }
    }
}
