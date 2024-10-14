using FireRingStudio.Extensions;
using FMOD;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.FMODIntegration
{
    [RequireComponent(typeof(StudioEventEmitter), typeof(SphereCollider))]
    public class FMODNoise : MonoBehaviour
    {
        private SphereCollider _sphereCollider;
        private StudioEventEmitter _eventEmitter;

        private float _elapsedTime;
        private float _radius;
        private float _audioLength;
        private float _t;

        private StudioEventEmitter EventEmitter
        {
            get
            {
                if (_eventEmitter == null)
                {
                    _eventEmitter = gameObject.GetOrAddComponent<StudioEventEmitter>();
                }

                return _eventEmitter;
            }
        }
        private SphereCollider SphereCollider
        {
            get
            {
                if (_sphereCollider == null)
                {
                    _sphereCollider = gameObject.GetOrAddComponent<SphereCollider>();
                    _sphereCollider.enabled = false;
                    _sphereCollider.radius = 0f;
                    _sphereCollider.isTrigger = true;
                }

                return _sphereCollider;
            }
        }


        private void Update()
        {
            if (!GameManager.InGame || _radius <= 0f || _t >= 1f)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= 0.2f)
            {
                SphereCollider.enabled = true;
            }

            _t += Time.deltaTime / _audioLength;
            SphereCollider.radius = Mathf.Lerp(_radius, 0f, _t);

            if (SphereCollider.radius == 0f)
            {
                SphereCollider.enabled = false;
            }
        }

        public void SetNoise(float radius)
        {
            _radius = radius;
            _audioLength = 0f;
            _t = 0f;
            _elapsedTime = 0f;

            SphereCollider.enabled = false;

            if (radius > 0f && EventEmitter.EventDescription.getLength(out int length) == RESULT.OK)
            {
                _audioLength = length / 1000f;
            }
        }
    }
}