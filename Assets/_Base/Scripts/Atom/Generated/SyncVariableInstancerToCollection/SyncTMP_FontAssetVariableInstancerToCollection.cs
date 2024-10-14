using UnityEngine;
using UnityAtoms.BaseAtoms;
using TMPro;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type TMPro.TMP_FontAsset to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync TMP_FontAsset Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncTMP_FontAssetVariableInstancerToCollection : SyncVariableInstancerToCollection<TMPro.TMP_FontAsset, TMP_FontAssetVariable, TMP_FontAssetVariableInstancer> { }
}
