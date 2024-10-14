using System.Collections;
using FireRingStudio.Extensions;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

namespace FireRingStudio.Animation
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Vector2 _flickerTimeRange = Vector2.up * 0.1f;
        [SerializeField] private Vector2 _intensityScaleRange = Vector2.up;
        [SerializeField] private bool _startFlickerOnEnable = true;

        [Header("Audios")]
        [SerializeField] private EventReference _turnOnSFX;
        [SerializeField] private EventReference _turnOffSFX;

        private Light[] _lights;

        private float[] _defaultIntensities;
        
        public bool On { get; private set; }
        public bool IsFlickering { get; private set; }
        
        [Space]
        public UnityEvent onTurnOn;
        public UnityEvent onTurnOff;


        private void Awake()
        {
            LoadLights();
        }

        private void OnEnable()
        {
            if (_startFlickerOnEnable)
            {
                StartFlicker();
            }
        }

        private void OnDisable()
        {
            StopFlicker();
        }
        
        [HideIf("IsFlickering")]
        [Button]
        public void StartFlicker()
        {
            StopAllCoroutines();
            StartCoroutine(FlickerRoutine());
            IsFlickering = true;
        }

        [ShowIf("IsFlickering")]
        [Button]
        public void StopFlicker()
        {
            StopAllCoroutines();
            IsFlickering = false;
        }

        [Button]
        public void Flick()
        {
            StartCoroutine(FlickRoutine());
        }

        public void TurnOn()
        {
            if (On || _lights == null)
            {
                return;
            }

            float intensityScale = UnityEngine.Random.Range(_intensityScaleRange.x, _intensityScaleRange.y);
            
            for (int i = 0; i < _lights.Length; i++)
            {
                _lights[i].intensity = _defaultIntensities[i] * intensityScale;
                _lights[i].enabled = true;
            }

            On = true;

            onTurnOn?.Invoke();

            if (!_turnOnSFX.IsNull)
            {
                _turnOnSFX.Play(transform.position);
            }
        }

        public void TurnOff()
        {
            if (!On || _lights == null)
            {
                return;
            }

            foreach (Light light in _lights)
            {
                light.enabled = false;
            }

            On = false;

            onTurnOff?.Invoke();

            if (!_turnOffSFX.IsNull)
            {
                _turnOffSFX.Play(transform.position);
            }
        }

        private IEnumerator FlickRoutine()
        {
            TurnOn();

            yield return new WaitForSeconds(_flickerTimeRange.RandomValue());
            
            TurnOff();
        }

        private IEnumerator FlickerRoutine()
        {
            while (true)
            {
                yield return FlickRoutine();
            }
        }

        private void LoadLights()
        {
            _lights = GetComponentsInChildren<Light>();

            _defaultIntensities = new float[_lights.Length];
            for (int i = 0; i < _lights.Length; i++)
            {
                _defaultIntensities[i] = _lights[i].intensity;
            }
        }
    }
}