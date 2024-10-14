using System.Collections.Generic;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Physics
{
    public abstract class Caster : Detector
    {
        [SerializeField] protected bool _useNonAlloc = true;
        [ShowIf("_useNonAlloc")]
        [SerializeField, Min(1)] protected int _maxAmountOfHits = 8;
        
        [Space]
        [SerializeField, Min(1)] protected int _updateInterval = 1;
        
        public List<RaycastHit> Hits { get; private set; }
        
        private bool CanUpdate => UpdateManager.FixedUpdateCount % _updateInterval == 0;
        
        
        private void FixedUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }
            
            UpdateDetector();
        }

        [Button("Update")]
        public virtual void UpdateDetector()
        {
            bool wasDetecting = IsDetecting;

            Hits = GetDetectedHits();
            _colliders.Clear();
            _colliders.AddRange(Hits.Select(hit => hit.collider));

            TryInvokeEvents(wasDetecting);
        }

        public bool TryGetClosestHit(out RaycastHit hit)
        {
            hit = default;
            if (Hits == null || Hits.Count == 0)
            {
                return false;
            }
            
            hit = Hits[0];
            if (Hits.Count == 1)
            {
                return true;
            }

            foreach (RaycastHit other in Hits)
            {
                if (other.distance < hit.distance)
                {
                    hit = other;
                }
            }
            
            return true;
        }

        private List<RaycastHit> GetDetectedHits()
        {
            int amount = Cast(out RaycastHit[] results, _targetLayerMask);

            List<RaycastHit> hits = new List<RaycastHit>();
            for (int i = 0; i < amount; i++)
            {
                RaycastHit hit = results[i];
                if (CanDetect(hit.collider, hit.point))
                {
                    hits.Add(hit);
                }
            }

            return hits;
        }

        protected abstract int Cast(out RaycastHit[] results, int layerMask = ~0);
    }
}