using FireRingStudio.Extensions;
using FMODUnity;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.FMODIntegration
{
    public class AttributeFMODAudioSource : MonoBehaviour
    {
        [FormerlySerializedAs("_audio")]
        [SerializeField] private EventReference _audioReference;
        [SerializeField] private FloatReference _floatReference;
        [SerializeField] private FloatReference _maxFloatReference = new FloatReference(100f);

        [Header("Settings")]
        [SerializeField] private string _parameterName;
        [FormerlySerializedAs("_percentageRangeToPlay")]
        [SerializeField] private Vector2 _playPercentRange = Vector2.up;
        [SerializeField] private bool _stopOnDisable;

        private StudioEventEmitter _eventEmitter;


        private void Update()
        {
            if (_floatReference == null || _maxFloatReference == null)
            {
                return;
            }

            float percentage = Mathf.Lerp(0f, 1f, _floatReference.Value / _maxFloatReference.Value);
            if (percentage >= _playPercentRange.x && percentage <= _playPercentRange.y)
            {
                Play();
                UpdateParameter(percentage);
            }
            else
            {
                Stop();
            }
        }

        private void OnDisable()
        {
            if (_stopOnDisable)
            {
                Stop();
            }
        }

        private void OnDestroy()
        {
            ReleaseAudioToPool();
        }

        private void Play()
        {
            if (_audioReference.IsNull)
            {
                return;
            }

            if (_eventEmitter == null)
            {
                _eventEmitter = _audioReference.Play(transform.position, false, false);
            }
            else if (!_eventEmitter.IsPlaying())
            {
                _eventEmitter.Play();
            }
        }

        private void Stop()
        {
            if (_eventEmitter != null)
            {
                _eventEmitter.Stop();
            }
        }

        private void UpdateParameter(float percentage)
        {
            if (_eventEmitter != null && _parameterName != null)
            {
                _eventEmitter.SetParameter(_parameterName, percentage);
            }
        }

        private void ReleaseAudioToPool()
        {
            if (_eventEmitter != null)
            {
                _eventEmitter.ReleaseToPoolOnStop();
                _eventEmitter = null;
            }
        }
    }
}