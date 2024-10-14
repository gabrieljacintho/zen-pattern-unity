using FireRingStudio.Physics;
using UnityEngine;

namespace FireRingStudio.Surface
{
    public class SurfaceEffectArea : HitArea
    {
        [Header("Surface Effect")]
        [SerializeField] private LayerMask _layerMask = ~0;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.Ignore;
        [SerializeField] private SurfaceEffectType _surfaceEffectType;
        [SerializeField] private string _surfaceEffectId;

        protected override LayerMask LayerMask => _layerMask;
        protected override QueryTriggerInteraction QueryTriggerInteraction => _queryTriggerInteraction;
        
        
        protected override void OnHit(Collider _, RaycastHit hit)
        {
            SurfaceManager.SpawnEffect(hit, _surfaceEffectType, _surfaceEffectId);
        }
    }
}