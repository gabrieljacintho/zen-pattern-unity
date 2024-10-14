using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using FireRingStudio.Extensions;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio/FMOD")]
    [TaskDescription("Play a FMOD event. Returns success.")]
    public class PlayFMODAudio : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        [SerializeField] private SharedGameObject _targetGameObject;
        [SerializeField] private SharedString _path;
        [SerializeField] private SharedBool _attach = true;
        [SerializeField] private SharedBool _allowFadeout = true;
        [SerializeField] private EmitterGameEvent _stopEvent;
        [SerializeField] private SharedBool _releaseToPoolOnStop;
        [SerializeField] private SharedBool _selfReleaseToPool;
        [SerializeField] private SharedFloat _noiseRadius;
        [SerializeField] private SharedBool _playOnlyIfIsNotPlaying;
        [SerializeField] private SharedStudioEventEmitter _storeEventEmitter;

        private Coroutine _releaseToPoolOnStopRoutine;

        private GameObject CurrentGameObject => _targetGameObject.Value != null ? _targetGameObject.Value : gameObject;


        public override TaskStatus OnUpdate()
        {
            if (string.IsNullOrEmpty(_path.Value))
            {
                return TaskStatus.Success;
            }

            if (_playOnlyIfIsNotPlaying.Value && _storeEventEmitter.Value != null && _storeEventEmitter.Value.IsPlaying())
            {
                return TaskStatus.Success;
            }

            StopReleaseToPoolOnStop();

            if (_storeEventEmitter.Value == null)
            {
                Vector3 position = CurrentGameObject.transform.position;
                Transform parent = _attach.Value ? transform : null;

                EventReference eventReference = new EventReference();
                eventReference.Guid = RuntimeManager.PathToGUID(_path.Value);
#if UNITY_EDITOR
                eventReference.Path = _path.Value;
#endif
                _storeEventEmitter.Value = eventReference.Play(position, parent, _allowFadeout.Value, _releaseToPoolOnStop.Value, _noiseRadius.Value, _stopEvent);
            }
            else
            {
                _storeEventEmitter.Value.Play();
            }

            if (_releaseToPoolOnStop.Value)
            {
                StartReleaseToPoolOnStop();
            }

            return TaskStatus.Success;
        }

        private void StartReleaseToPoolOnStop()
        {
            if (_storeEventEmitter.Value == null)
            {
                return;
            }

            _releaseToPoolOnStopRoutine = _storeEventEmitter.Value.ReleaseToPoolOnStop(OnReleaseAudioToPool);
        }

        private void StopReleaseToPoolOnStop()
        {
            if (_releaseToPoolOnStopRoutine == null)
            {
                return;
            }

            Owner.StopCoroutine(_releaseToPoolOnStopRoutine);
            _releaseToPoolOnStopRoutine = null;
        }

        private void OnReleaseAudioToPool()
        {
            _releaseToPoolOnStopRoutine = null;
            _storeEventEmitter.Value = null;

            if (_selfReleaseToPool.Value)
            {
                gameObject.ReleaseToPool();
            }
        }

        public override void OnReset()
        {
            _targetGameObject = null;
            _path = null;
            _attach = null;
            _allowFadeout = null;
            _releaseToPoolOnStop = null;
            _selfReleaseToPool = null;
            _noiseRadius = null;
            _playOnlyIfIsNotPlaying = null;
            _storeEventEmitter = null;
        }
    }
}