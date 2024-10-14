using FireRingStudio.FPS.UseEquipment;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS.HealingItem
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Healing Item Data", fileName = "New Healing Item Data")]
    public class HealingItemData : UseEquipmentData
    {
        [FoldoutGroup("Heal")] public FloatReference PlayerHealthReference;
        [FoldoutGroup("Heal")] public FloatReference MaxPlayerHealthReference;
        [FoldoutGroup("Heal")] public FloatReference MinHealReference = new(20f);
        [FoldoutGroup("Heal")] public FloatReference MaxHealReference = new(35f);
        [FoldoutGroup("Heal")] public bool CanBeUsedWhenHealthIsFull = true;
        [FoldoutGroup("Heal")] public bool CanHealPoison = true;

        public float PlayerHealth => PlayerHealthReference?.Value ?? 0f;
        public float MaxPlayerHealth => MaxPlayerHealthReference?.Value ?? 0f;
        public float MinHeal => MinHealReference?.Value ?? 0f;
        public float MaxHeal => MaxHealReference?.Value ?? 0f;
    }
}
