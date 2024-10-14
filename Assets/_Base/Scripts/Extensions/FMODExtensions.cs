using System;
using System.Collections;
using FireRingStudio.FMODIntegration;
using FMOD;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class FMODExtensions
    {
        #region Play
        
        public static StudioEventEmitter Play(this EventReference audioReference, Vector3 position, Transform parent,
            bool allowFadeout = true, bool releaseToPoolOnStop = true, float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            return FMODAudioManager.Play(audioReference, position, parent, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }

        public static StudioEventEmitter Play(this EventReference audioReference, Vector3 position, Transform parent,
            EmitterGameEvent stopEvent, bool allowFadeout = true, bool releaseToPoolOnStop = true, float noiseRadius = 0f)
        {
            return FMODAudioManager.Play(audioReference, position, parent, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }

        public static StudioEventEmitter Play(this EventReference audioReference, Vector3 position, bool allowFadeout = true,
            bool releaseToPoolOnStop = true, float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            return FMODAudioManager.Play(audioReference, position, null, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }
        
        public static StudioEventEmitter Play(this EventReference audioReference, Transform parent, bool allowFadeout = true,
            bool releaseToPoolOnStop = true, float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            return FMODAudioManager.Play(audioReference, parent.position, parent, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }

        public static StudioEventEmitter Play(this EventReference audioReference, Transform parent, EmitterGameEvent stopEvent,
            bool allowFadeout = true, bool releaseToPoolOnStop = true, float noiseRadius = 0f)
        {
            return FMODAudioManager.Play(audioReference, parent.position, parent, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }

        public static StudioEventEmitter Play(this EventReference audioReference, bool allowFadeout = true, bool releaseToPoolOnStop = true,
            float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            return FMODAudioManager.Play(audioReference, default, null, null, allowFadeout, releaseToPoolOnStop, noiseRadius, stopEvent);
        }
        
        public static StudioEventEmitter PlayClone(this StudioEventEmitter audioSource, Vector3 position = default, Transform parent = null,
            bool releaseToPoolOnStop = true, float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            EventReference audioReference = audioSource.EventReference;
            ParamRef[] @params = audioSource.Params;
            bool allowFadeout = audioSource.AllowFadeout;
            
            return FMODAudioManager.Play(audioReference, position, parent, @params, allowFadeout, releaseToPoolOnStop, noiseRadius, audioSource.StopEvent);
        }
        
        #endregion
        
        #region Getters
        
        public static float GetSeconds(this StudioEventEmitter audioSource)
        {
            RESULT result = audioSource.EventDescription.getLength(out int milliseconds);
            if (result != RESULT.OK)
            {
                return 0f;
            }

            return milliseconds / 1000f;
        }

        public static float GetElapsedSeconds(this StudioEventEmitter audioSource)
        {
            RESULT result = audioSource.EventInstance.getTimelinePosition(out int elapsedMilliseconds);
            if (result != RESULT.OK)
            {
                return 0f;
            }
            
            return elapsedMilliseconds / 1000f;
        }

        public static float GetRemainingSeconds(this StudioEventEmitter audioSource)
        {
            float seconds = audioSource.GetSeconds();
            float elapsedSeconds = audioSource.GetElapsedSeconds();
            
            return seconds - elapsedSeconds;
        }
        
        public static bool IsPaused(this StudioEventEmitter audioSource)
        {
            bool paused = false;
            if (audioSource.EventInstance.isValid())
            {
                audioSource.EventInstance.getPaused(out paused);
            }

            return paused;
        }
        
        #endregion
        
        #region Setters
        
        public static void SetPaused(this StudioEventEmitter audioSource, bool paused)
        {
            if (audioSource.EventInstance.isValid())
            {
                audioSource.EventInstance.setPaused(paused);
            }
        }
        
        #endregion

        public static void ReleaseToPool(this StudioEventEmitter audioSource)
        {
            if (audioSource.IsPlaying())
            {
                audioSource.Stop();
            }

            audioSource.gameObject.ReleaseToPool();
        }
        
        public static Coroutine ReleaseToPoolOnStop(this StudioEventEmitter audioSource, Action onRelease = null)
        {
            IEnumerator Routine()
            {
                yield return new WaitWhile(audioSource.IsNotStopped);
            
                audioSource.gameObject.ReleaseToPool();
                onRelease?.Invoke();
            }

            return UpdateManager.Instance != null ? UpdateManager.Instance.StartCoroutine(Routine()) : null;
        }

        public static bool IsNotStopped(this StudioEventEmitter eventEmitter)
        {
            if (!eventEmitter.EventInstance.isValid() || eventEmitter.EventInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state) != RESULT.OK)
            {
                return false;
            }

            return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
        }

        public static void UpdatePosition(this StudioEventEmitter audioSource)
        {
            if (audioSource.EventInstance.isValid())
            {
                audioSource.EventInstance.set3DAttributes(audioSource.gameObject.To3DAttributes());
            }
        }
    }
}