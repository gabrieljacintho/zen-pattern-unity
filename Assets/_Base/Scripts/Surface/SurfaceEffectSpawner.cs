using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Surface
{
    public class SurfaceEffectSpawner : MonoBehaviour
    {
        [SerializeField] private SurfaceEffectType _type;
        [SerializeField] private string _surfaceEffectId;
        [SerializeField] private bool _spawnOnEnable;
        
        [Header("Raycast")]
        [SerializeField] private bool _useRaycast;
        [ShowIf("_useRaycast")]
        [SerializeField] private Transform _rayTransform;
        [ShowIf("@_useRaycast && _rayTransform == null")]
        [SerializeField] private string _rayTransformId = "look";
        [ShowIf("_useRaycast")]
        [SerializeField] private float _maxDistance = 1f;
        [ShowIf("_useRaycast")]
        [SerializeField] private LayerMask _layerMask = ~0;
        [ShowIf("_useRaycast")]
        [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

        private Transform RayTransform
        {
            get
            {
                if (_rayTransform == null)
                {
                    _rayTransform = ComponentID.FindComponentWithID<Transform>(_rayTransformId, true);
                }

                return _rayTransform;
            }
        }


        private void OnEnable()
        {
            if (_spawnOnEnable)
            {
                SpawnEffect();
            }
        }

        public void SpawnEffect()
        {
            if (_useRaycast)
            {
                if (Raycast(out RaycastHit hit))
                {
                    SurfaceManager.SpawnEffect(hit, _type, _surfaceEffectId);
                }
            }
            else
            {
                SurfaceManager.SpawnEffect(gameObject, _type, _surfaceEffectId);
            }
        }

        public void SpawnEffect(GameObject targetObject)
        {
            SurfaceManager.SpawnEffect(targetObject, _type, _surfaceEffectId);
        }

        public void SpawnEffect(Collider targetCollider)
        {
            SpawnEffect(targetCollider.gameObject);
        }

        private bool Raycast(out RaycastHit hit)
        {
            hit = default;

            Ray ray = new Ray(RayTransform.position, RayTransform.forward);
            if (!UnityEngine.Physics.Raycast(ray, out hit, _maxDistance, _layerMask.value, _triggerInteraction))
            {
                return false;
            }

            if (!_layerMask.Contains(hit.transform.gameObject.layer))
            {
                return false;
            }

            return true;
        }
    }
}