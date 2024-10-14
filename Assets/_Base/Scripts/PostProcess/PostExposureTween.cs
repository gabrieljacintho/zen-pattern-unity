using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FireRingStudio.PostProcess
{
    public class PostExposureTween : MonoBehaviour
    {
        [SerializeField] private PostProcessProfile _profile;
        [SerializeField] private float _from;
        [SerializeField] private float _to = 1f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private bool _startOnEnable;

        private ColorGrading _colorGrading;
        private bool _running;
        private float _t;

        private ColorGrading ColorGrading => GetColorGrading();


        protected void Awake()
        {
            if (ColorGrading != null)
            {
                ColorGrading.active = true;
                ColorGrading.enabled.value = true;
                ColorGrading.postExposure.overrideState = true;
            }
        }

        private void OnEnable()
        {
            if (_startOnEnable)
            {
                StartTween();
            }
        }

        private void Update()
        {
            if (GameManager.IsPaused || !_running || ColorGrading == null)
            {
                return;
            }

            _t += Time.deltaTime / _duration;
            float time = _transitionCurve.Evaluate(_t);
            float postExposure = Mathf.Lerp(_from, _to, time);

            ColorGrading.postExposure.value = postExposure;

            if (_t >= 1f)
            {
                _running = false;
            }
        }

        [HideIf("_running")]
        [Button]
        public void StartTween()
        {
            _running = true;
            _t = 0f;

            if (ColorGrading != null)
            {
                ColorGrading.postExposure.value = _from;
            }
        }

        [ShowIf("_running")]
        [Button]
        public void StopTween()
        {
            _running = false;
        }

        private ColorGrading GetColorGrading()
        {
            if (_colorGrading != null)
            {
                return _colorGrading;
            }

            if (_profile != null && !_profile.TryGetSettings(out _colorGrading))
            {
                _colorGrading = _profile.AddSettings<ColorGrading>();
            }

            return _colorGrading;
        }
    }
}