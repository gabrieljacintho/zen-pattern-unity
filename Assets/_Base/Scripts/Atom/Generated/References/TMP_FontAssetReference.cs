using System;
using UnityAtoms.BaseAtoms;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `TMPro.TMP_FontAsset`. Inherits from `AtomReference&lt;TMPro.TMP_FontAsset, TMP_FontAssetPair, TMP_FontAssetConstant, TMP_FontAssetVariable, TMP_FontAssetEvent, TMP_FontAssetPairEvent, TMP_FontAssetTMP_FontAssetFunction, TMP_FontAssetVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class TMP_FontAssetReference : AtomReference<
        TMPro.TMP_FontAsset,
        TMP_FontAssetPair,
        TMP_FontAssetConstant,
        TMP_FontAssetVariable,
        TMP_FontAssetEvent,
        TMP_FontAssetPairEvent,
        TMP_FontAssetTMP_FontAssetFunction,
        TMP_FontAssetVariableInstancer>, IEquatable<TMP_FontAssetReference>
    {
        public TMP_FontAssetReference() : base() { }
        public TMP_FontAssetReference(TMPro.TMP_FontAsset value) : base(value) { }
        public bool Equals(TMP_FontAssetReference other) { return base.Equals(other); }
        protected override bool ValueEquals(TMPro.TMP_FontAsset other)
        {
            throw new NotImplementedException();
        }
    }
}
