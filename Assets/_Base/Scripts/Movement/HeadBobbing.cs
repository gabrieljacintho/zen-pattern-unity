using FireRingStudio.Operations;
using FireRingStudio.Surface;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace FireRingStudio.Movement
{
    [Serializable]
    public struct HeadBobbingSettings
    {
        public float Amplitude;
        public float Frequency;
    }

    public class HeadBobbing : MonoBehaviour
    {
        [SerializeField] private Transform _lookTransform;

        [Header("States")]
        [SerializeField] private MovementStateValues<HeadBobbingSettings> _states;

        [Header("Settings")]
        [SerializeField] private float _returnSpeed = 10f;
        [SerializeField] private bool _canPlayFootstepSFX = true;
        
        [Space]
        [ReadOnly] public FloatMultiplication AmplitudeScale = new(1f);
        [ReadOnly] public FloatMultiplication FrequencyScale = new(1f);

        private PlayerMovement _playerMovement;
        private FootstepEffectPlayer _footstepPlayer;
        
        private Vector3 _initialPosition;

        private int _lastFootstepDirection;
        private bool _overrideSettings;
        private HeadBobbingSettings _newSettings;


        private void Awake()
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
            _footstepPlayer = GameObjectID.FindComponentInChildrenWithID<FootstepEffectPlayer>(GameObjectID.PlayerID, true);
            _initialPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!GameManager.InAnyGameState || _playerMovement == null)
            {
                return;
            }

            if (GameManager.InGame)
            {
                HeadBobbingSettings settings = _overrideSettings ? _newSettings : _states.GetValue(_playerMovement.CurrentState);
                Swing(settings);
            }

            ReturnToInitialPosition();

            LookToForward();
        }

        public void OverrideSettings(HeadBobbingSettings newSettings)
        {
            _overrideSettings = true;
            _newSettings = newSettings;
        }

        public void DisableOverrideSettings()
        {
            _overrideSettings = false;
        }

        private void Swing(HeadBobbingSettings settings)
        {
            settings.Amplitude *= AmplitudeScale.Result;
            settings.Frequency *= FrequencyScale.Result;

            settings.Amplitude /= 1000f;

            Vector3 position = Vector3.zero;
            position.y += Mathf.Sin(Time.time * settings.Frequency) * settings.Amplitude;
            position.x += Mathf.Cos(Time.time * settings.Frequency / 2) * settings.Amplitude * 2;

            transform.localPosition += position * Time.deltaTime;

            if (_canPlayFootstepSFX && (_playerMovement.CurrentState == MovementState.Walking || _playerMovement.CurrentState == MovementState.Sprinting))
            {
                PlayFootstepSFX(position.x > 0f ? 1 : -1);
            }
        }

        private void ReturnToInitialPosition()
        {
            if (transform.localPosition == _initialPosition)
            {
                return;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, _returnSpeed * Time.deltaTime);
        }
        
        private void LookToForward()
        {
            if (_lookTransform != null)
            {
                transform.LookAt(transform.position + _lookTransform.forward * 15f);
            }
        }

        private void PlayFootstepSFX(int direction = 0)
        {
            if (_footstepPlayer == null || direction == _lastFootstepDirection)
            {
                return;
            }
            
            _footstepPlayer.TryPlay(direction);
            
            _lastFootstepDirection = direction;
        }
    }
}