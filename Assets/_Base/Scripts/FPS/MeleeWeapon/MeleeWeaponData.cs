using FireRingStudio.FPS.UseEquipment;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS.MeleeWeapon
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Melee Weapon Data", fileName = "New Melee Weapon Data")]
    public class MeleeWeaponData : UseEquipmentData
    {
        [FoldoutGroup("Attack")] public DamageParameters DamageParameters;
        [FoldoutGroup("Attack")] public float DisableDamageDelay = 0.1f;
    }
}
