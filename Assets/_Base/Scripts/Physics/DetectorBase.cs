using System.Collections.Generic;
using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.Physics
{
    public abstract class DetectorBase : MonoBehaviour
    {
        [SerializeField] protected LayerMask _targetLayerMask = ~0;
        [SerializeField] protected LayerMask _obstacleLayerMask;
        [SerializeField] protected QueryTriggerInteraction _triggerInteraction;
        [SerializeField] protected string _requiredTag;
        
        
        protected virtual void Start()
        {
            // Show enabled toggle in editor
        }
        
        public virtual bool CanDetect(Collider targetCollider)
        {
            if (!isActiveAndEnabled || !PhysicsHelper.CanDetect(targetCollider.gameObject, _targetLayerMask, _requiredTag))
            {
                return false;
            }

            return !PhysicsHelper.HasObstacleBetween(transform.position, targetCollider, _obstacleLayerMask, _triggerInteraction);
        }

        public virtual bool CanDetect(Collider targetCollider, Vector3 contactPoint)
        {
            if (!isActiveAndEnabled || !PhysicsHelper.CanDetect(targetCollider.gameObject, _targetLayerMask, _requiredTag))
            {
                return false;
            }

            List<Collider> excludedColliders = new() { targetCollider };
            
            return !PhysicsHelper.HasObstacleBetween(transform.position, contactPoint, _obstacleLayerMask, _triggerInteraction, excludedColliders);
        }
    }
}