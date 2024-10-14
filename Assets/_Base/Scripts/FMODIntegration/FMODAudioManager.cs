using System;
using FireRingStudio.Pool;
using FMODUnity;
using System.IO;
using FireRingStudio.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;
using FireRingStudio.Cache;

namespace FireRingStudio.FMODIntegration
{
    public static class FMODAudioManager
    {
        private static GameObject s_audioPrefab;

        private static readonly ComponentCacheGeneric<StudioEventEmitter> s_audioSources = new();
        private static readonly ComponentCacheGeneric<FMODNoise> s_noises = new();

        public static bool IsPaused { get; private set; }
        
        private static GameObject AudioPrefab
        {
            get
            {
                if (s_audioPrefab == null)
                {
                    s_audioPrefab = CreateAudioPrefab();
                }

                return s_audioPrefab;
            }
            set => s_audioPrefab = value;
        }
        
        
        public static StudioEventEmitter Play(EventReference audioReference, Vector3 position = default,
            Transform parent = null, ParamRef[] @params = null, bool allowFadeout = true, bool releaseToPoolOnStop = true,
            float noiseRadius = 0f, EmitterGameEvent stopEvent = EmitterGameEvent.None)
        {
            StudioEventEmitter audioSource = GetAudioSource(audioReference, position, parent, @params, allowFadeout);
            audioSource.StopEvent = stopEvent;
            audioSource.EventReference = audioReference;
            audioSource.Params = @params ?? Array.Empty<ParamRef>();
            audioSource.AllowFadeout = allowFadeout;
            audioSource.Preload = true;
            audioSource.enabled = true;
            audioSource.UpdatePosition();
            audioSource.gameObject.SetActive(true);
            audioSource.Play();

            s_noises.TryGetComponent(audioSource.gameObject, out FMODNoise noise);
            noise.SetNoise(noiseRadius);

            if (releaseToPoolOnStop)
            {
                audioSource.ReleaseToPoolOnStop();
            }
            
            return audioSource;
        }
        
        public static void SetPaused(bool paused)
        {
            foreach (StudioEventEmitter eventEmitter in s_audioSources.ComponentByObject.Values)
            {
                if (eventEmitter != null)
                {
                    eventEmitter.SetPaused(paused);
                }
            }
            
            IsPaused = paused;
        }

        public static void Pause() => SetPaused(true);

        public static void Continue() => SetPaused(false);

        #region Getters
        
        private static StudioEventEmitter GetAudioSource(EventReference audioReference, Vector3 position = default,
            Transform parent = null, ParamRef[] @params = null, bool allowFadeout = true)
        {
            string poolTag = "Audio";
#if UNITY_EDITOR
            poolTag += "_" + new DirectoryInfo(audioReference.Path).Name;
#else
            poolTag += "_" + audioReference;
#endif

            if (@params != null)
            {
                foreach (ParamRef param in @params)
                {
                    poolTag += "_" + param.Value;
                }
            }
            
            if (!PoolManager.HasValidPoolWithTag(poolTag))
            {
                PoolManager.GetPool(poolTag, AudioPrefab);
            }
            
            GameObject instance = PoolManager.Get(poolTag, position, Quaternion.identity, parent).gameObject;
            instance.name = poolTag;

            s_audioSources.TryGetComponent(instance, out StudioEventEmitter audioSource);

            return audioSource;
        }
        
        #endregion
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            AudioPrefab = CreateAudioPrefab();
            
            GameManager.GamePaused += Pause;
            GameManager.GameUnpaused += Continue;
        }

        private static GameObject CreateAudioPrefab()
        {
            GameObject prefab = new GameObject("AudioPrefab");
            Object.DontDestroyOnLoad(prefab);
            prefab.layer = LayerMask.NameToLayer("Audio");
            
            StudioEventEmitter audioSource = prefab.AddComponent<StudioEventEmitter>();
            audioSource.enabled = false;

            SphereCollider sphereCollider = prefab.AddComponent<SphereCollider>();
            sphereCollider.enabled = false;
            sphereCollider.radius = 0f;
            sphereCollider.isTrigger = true;

            Rigidbody rigidbody = prefab.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            prefab.AddComponent<FMODNoise>();

            return prefab;
        }
    }
}