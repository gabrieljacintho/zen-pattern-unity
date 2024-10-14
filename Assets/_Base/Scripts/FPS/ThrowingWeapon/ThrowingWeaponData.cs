using FireRingStudio.FPS.UseEquipment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.ThrowingWeapon
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Throwing Weapon Data", fileName = "New Throwing Weapon Data")]
    public class ThrowingWeaponData : UseEquipmentData
    {
        [FoldoutGroup("Throw")] public GameObject ProjectilePrefab;
        [FoldoutGroup("Throw")] public ThrowParameters ThrowParameters = new ThrowParameters(12f, 0.01f, 1f);
    }
}
