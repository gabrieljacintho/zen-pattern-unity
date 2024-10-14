using FireRingStudio.Extensions;
using FireRingStudio.Movement;
using FireRingStudio.Physics;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Vitals
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Stamina : MonoBehaviour
    {
        public FloatReference staminaReference = new(100f);
        public FloatReference maxStaminaReference = new(100f);
        [ES3NonSerializable] public Detector groundDetector;

        [Header("Settings")]
        [ES3NonSerializable] public float increaseSpeed = 3f;
        [ES3NonSerializable] public float decreaseSpeed = 1f;
        [ES3NonSerializable] public float speedThreshold = 1f;
        [ES3NonSerializable] public float restThreshold = 60f;
        [ES3NonSerializable] public bool restFullOnEnable;

        [Header("Audios")]
        [ES3NonSerializable] public EventReference normalBreathAudio;
        [ES3NonSerializable] public EventReference tiredBreathAudio;
        
        private StudioEventEmitter _normalBreathAudioSource;
        private StudioEventEmitter _pantingBreathAudioSource;

        [ES3Serializable]
        public float CurrentStamina
        {
            get => staminaReference != null ? staminaReference.Value : 0f;
            set
            {
                if (staminaReference == null)
                {
                    staminaReference = new FloatReference(0f);
                }

                staminaReference.Value = Mathf.Clamp(value, 0f, MaxStamina);
            }
        }
        public float MaxStamina => maxStaminaReference != null ? maxStaminaReference.Value : 0f;

        [ES3Serializable]
        public bool IsTired { get; set; }

        public PlayerMovement PlayerMovement { get; private set; }

        [Space]
        [ES3NonSerializable] public UnityEvent onGetTired;
        [ES3NonSerializable] public UnityEvent onRest;


        private void Awake()
        {
            PlayerMovement = GetComponent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (restFullOnEnable)
            {
                RestFull();
            }
            
            UpdateAudio();
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            float speed = GetCurrentSpeed();

            if (speed < speedThreshold)
            {
                CurrentStamina += increaseSpeed * Time.deltaTime;
            }
            else
            {
                CurrentStamina -= decreaseSpeed * speed * Time.deltaTime;
            }

            if (CurrentStamina < 1f && !IsTired)
            {
                GetTired();
            }
            else if (CurrentStamina >= restThreshold && IsTired)
            {
                Rest();
            }
        }
        
        private void OnDisable()
        {
            ReleaseNormalBreathAudioToPool();
            ReleasePantingBreathAudioToPool();
        }

        public void UpdateAudio()
        {
            if (!normalBreathAudio.IsNull)
            {
                if (IsTired)
                {
                    StopNormalBreathAudio();
                }
                else
                {
                    PlayNormalBreathAudio();
                }
            }

            if (!tiredBreathAudio.IsNull)
            {
                if (IsTired)
                {
                    PlayPantingBreathAudio();
                }
                else
                {
                    StopPantingBreathAudio();
                }
            }
        }
        
        #region GetTired/Rest

        [HideIf("IsTired")]
        [Button]
        public void GetTired()
        {
            if (CurrentStamina >= 1f)
                CurrentStamina = 0f;

            IsTired = true;
            
            UpdateAudio();
            
            onGetTired?.Invoke();
        }
        
        public void Rest(bool full = false)
        {
            if (full || CurrentStamina < restThreshold)
                CurrentStamina = MaxStamina;
            
            IsTired = false;
            
            UpdateAudio();
            
            onRest?.Invoke();
        }

        [ShowIf("@CurrentStamina < MaxStamina")]
        [Button]
        public void RestFull() => Rest(true);
        
        #endregion

        #region Audios

        private void PlayNormalBreathAudio()
        {
            if (normalBreathAudio.IsNull)
            {
                return;
            }

            if (_normalBreathAudioSource == null)
            {
                _normalBreathAudioSource = normalBreathAudio.Play(true, false);
            }
            else if (!_normalBreathAudioSource.IsPlaying())
            {
                _normalBreathAudioSource.Play();
            }
        }
        
        private void StopNormalBreathAudio()
        {
            if (_normalBreathAudioSource != null)
            {
                _normalBreathAudioSource.Stop();
            }
        }

        private void ReleaseNormalBreathAudioToPool()
        {
            if (_normalBreathAudioSource != null)
            {
                _normalBreathAudioSource.ReleaseToPool();
                _normalBreathAudioSource = null;
            }
        }

        private void PlayPantingBreathAudio()
        {
            if (tiredBreathAudio.IsNull)
            {
                return;
            }

            if (_pantingBreathAudioSource == null)
            {
                _pantingBreathAudioSource = tiredBreathAudio.Play(true, false);
            }
            else if (!_pantingBreathAudioSource.IsPlaying())
            {
                _pantingBreathAudioSource.Play();
            }
        }

        private void StopPantingBreathAudio()
        {
            if (_pantingBreathAudioSource != null)
            {
                _pantingBreathAudioSource.Stop();
            }
        }
        
        private void ReleasePantingBreathAudioToPool()
        {
            if (_pantingBreathAudioSource != null)
            {
                _pantingBreathAudioSource.ReleaseToPool();
                _pantingBreathAudioSource = null;
            }
        }

        #endregion
        
        #region Getters
        
        private float GetCurrentSpeed()
        {
            if (groundDetector != null && !groundDetector.IsDetecting)
            {
                return 0f;
            }

            return PlayerMovement != null ? PlayerMovement.CurrentSpeed : 0f;
        }

        #endregion
    }
}