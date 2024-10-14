using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(Light))]
    public class LightMoveTowards : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Light _light;

        private float _defaultIntensity;
        private float _targetIntensity;

        public float TargetIntensity
        {
            get => _targetIntensity;
            set => _targetIntensity = value;
        }


        private void Awake()
        {
            _light = GetComponent<Light>();
            _defaultIntensity = _light.intensity;
            _targetIntensity = _light.intensity;
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            _light.intensity = Mathf.MoveTowards(_light.intensity, _targetIntensity, _speed * Time.deltaTime);
        }

        public void ResetTargetIntensity()
        {
            _targetIntensity = _defaultIntensity;
        }
    }
}