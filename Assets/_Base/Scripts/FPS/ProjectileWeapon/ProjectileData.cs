using FireRingStudio.Inventory;
using FireRingStudio.Surface;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Projectile Data", fileName = "New Projectile Data")]
    public class ProjectileData : ItemData
    {
        [Header("Projectile")]
        public GameObject ProjectilePrefab;
        public GameObject CasingPrefab;
        public Cartridge CartridgePrefab;
        
        [Header("Impact")]
        public float MaxDistance = 0.5f;
        public LayerMask LayerMask = ~0;
        public QueryTriggerInteraction QueryTriggerInteraction;
        public float PenetrationOffset;
        
        [Header("Surface Effect")]
        public SurfaceEffectType ImpactEffectType = SurfaceEffectType.BulletImpact;
        public string ImpactEffectId;
    }
}