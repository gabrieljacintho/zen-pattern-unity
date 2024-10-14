using System;
using UnityEngine;
using TMPro;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;TMPro.TMP_FontAsset&gt;`. Inherits from `IPair&lt;TMPro.TMP_FontAsset&gt;`.
    /// </summary>
    [Serializable]
    public struct TMP_FontAssetPair : IPair<TMPro.TMP_FontAsset>
    {
        public TMPro.TMP_FontAsset Item1 { get => _item1; set => _item1 = value; }
        public TMPro.TMP_FontAsset Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private TMPro.TMP_FontAsset _item1;
        [SerializeField]
        private TMPro.TMP_FontAsset _item2;

        public void Deconstruct(out TMPro.TMP_FontAsset item1, out TMPro.TMP_FontAsset item2) { item1 = Item1; item2 = Item2; }
    }
}