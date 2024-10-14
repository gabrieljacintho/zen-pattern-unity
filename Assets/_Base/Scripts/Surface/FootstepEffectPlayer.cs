using FireRingStudio.FMODIntegration;
using FireRingStudio.Cache;
using FireRingStudio.Physics;
using FireRingStudio.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Surface
{
    public class FootstepEffectPlayer : MonoBehaviour
    {
        [SerializeField] private Caster _groundCaster;
        [SerializeField] private string _surfaceEffectId;
        [SerializeField] private float _minInterval;

        private float _previousPlayTime;


        [Button]
        public void TryPlay(int direction = 0)
        {
            if (_minInterval > 0f && Time.time - _previousPlayTime < _minInterval)
            {
                return;
            }

            if (_groundCaster == null || !_groundCaster.IsDetecting)
            {
                return;
            }

            if (!_groundCaster.TryGetClosestHit(out RaycastHit hit))
            {
                return;
            }

            PooledObject effectObject = SurfaceManager.SpawnEffect(hit, SurfaceEffectType.Footstep, _surfaceEffectId, false);
            if (effectObject == null)
            {
                return;
            }

            FMODAudioSource audioSource = ComponentCacheManager.GetComponent<FMODAudioSource>(effectObject.gameObject);
            if (audioSource != null && audioSource.EventEmitter != null && audioSource.EventEmitter.EventInstance.isValid())
            {
                audioSource.EventEmitter.EventInstance.setParameterByName("Direction", direction);
            }

            _previousPlayTime = Time.time;
        }
    }
}