using FireRingStudio.Extensions;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.FMODIntegration
{
    public class FMODAudioSource : MonoBehaviour
    {
        [FormerlySerializedAs("_eventEmitter")]
        [FormerlySerializedAs("eventEmitter")]
        [SerializeField] private StudioEventEmitter _audioSourceReference;
        [FormerlySerializedAs("_eventReference")]
        [FormerlySerializedAs("eventReference")]
        [HideIf("@_audioSourceReference != null")]
        [SerializeField] private EventReference _audioReference;

        [Header("Settings")]
        [SerializeField] private bool _attach = true;
        [HideIf("@_audioSourceReference != null")]
        [SerializeField] private bool _allowFadeout = true;
        [HideIf("@_audioSourceReference != null")]
        [SerializeField] private EmitterGameEvent _stopEvent;
        [FormerlySerializedAs("_autoReleaseToPool")]
        [FormerlySerializedAs("autoReleaseToPool")]
        [SerializeField] private bool _releaseToPoolOnStop;
        [ShowIf("_releaseToPoolOnStop")]
        [SerializeField] private bool _selfReleaseToPool;
        [SerializeField] private float _noiseRadius;
        [SerializeField] private bool _playOnlyIfIsNotPlaying;
        
        [Space]
        [FormerlySerializedAs("playOnEnable")]
        [SerializeField] private bool _playOnEnable;
        [FormerlySerializedAs("stopOnDisable")]
        [SerializeField] private bool _stopOnDisable;

        private StudioEventEmitter _eventEmitter;
        private Coroutine _releaseToPoolOnStopRoutine;

        public StudioEventEmitter EventEmitter => _eventEmitter;
        

        private void OnEnable()
        {
            if (_playOnEnable)
            {
                Play();
            }
        }
        
        private void OnDisable()
        {
            if (_stopOnDisable)
            {
                Stop();
            }
        }

        [Button]
        public void Play()
        {
            if (_playOnlyIfIsNotPlaying && _eventEmitter != null && _eventEmitter.IsPlaying())
            {
                return;
            }

            StopReleaseToPoolOnStop();

            if (_eventEmitter == null)
            {
                Transform parent = _attach ? transform : null;
                
                if (_audioSourceReference != null)
                {
                    _eventEmitter = _audioSourceReference.PlayClone(transform.position, parent, false, _noiseRadius);
                }
                else if (!_audioReference.IsNull)
                {
                    _eventEmitter = _audioReference.Play(transform.position, parent, _allowFadeout, false, _noiseRadius, _stopEvent);
                }
            }
            else
            {
                _eventEmitter.Play();
            }
            
            if (_releaseToPoolOnStop)
            {
                StartReleaseToPoolOnStop();
            }
        }

        [Button]
        public void Stop()
        {
            if (_eventEmitter != null)
            {
                _eventEmitter.Stop();
            }
        }

        public void SetPaused(bool paused)
        {
            if (_eventEmitter != null)
            {
                _eventEmitter.SetPaused(paused);
            }
        }

        [Button]
        public void Pause() => SetPaused(true);

        [Button]
        public void Continue() => SetPaused(false);

        private void StartReleaseToPoolOnStop()
        {
            if (_eventEmitter == null)
            {
                return;
            }
            
            _releaseToPoolOnStopRoutine = _eventEmitter.ReleaseToPoolOnStop(OnReleaseAudioToPool);
        }

        private void StopReleaseToPoolOnStop()
        {
            if (_releaseToPoolOnStopRoutine == null)
            {
                return;
            }
            
            StopCoroutine(_releaseToPoolOnStopRoutine);
            _releaseToPoolOnStopRoutine = null;
        }
        
        private void OnReleaseAudioToPool()
        {
            _releaseToPoolOnStopRoutine = null;
            _eventEmitter = null;

            if (_selfReleaseToPool)
            {
                gameObject.ReleaseToPool();
            }
        }
    }
}