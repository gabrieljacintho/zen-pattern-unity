using UnityEngine;
using UnityAtoms.BaseAtoms;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type FireRingStudio.Vitals.Damage to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync Damage Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncDamageVariableInstancerToCollection : SyncVariableInstancerToCollection<FireRingStudio.Vitals.Damage, DamageVariable, DamageVariableInstancer> { }
}
