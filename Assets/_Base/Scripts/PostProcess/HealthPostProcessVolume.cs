using FireRingStudio.Vitals;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FireRingStudio.PostProcess
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class HealthPostProcessVolume : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Vector2 _weightRange = new Vector2(0f, 1f);
        [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private float _smoothTime = 0.3f;

        private PostProcessVolume _postProcessVolume;

        private float _currentVelocity;


        private void Awake()
        {
            _postProcessVolume = GetComponent<PostProcessVolume>();
        }

        private void OnEnable()
        {
            UpdateWeight();
        }

        private void LateUpdate()
        {
            float currentWeight = _postProcessVolume.weight;
            float targetWeight = GetWeight();

            _postProcessVolume.weight = Mathf.SmoothDamp(currentWeight, targetWeight, ref _currentVelocity, _smoothTime);
        }

        public void UpdateWeight()
        {
            _postProcessVolume.weight = GetWeight();
        }

        private float GetWeight()
        {
            if (_health == null)
            {
                return _weightRange.x;
            }

            float t = Mathf.Clamp01(_health.CurrentHealth / _health.MaxHealth);

            t = _transitionCurve.Evaluate(t);

            return Mathf.Lerp(_weightRange.x, _weightRange.y, t);
        }
    }
}