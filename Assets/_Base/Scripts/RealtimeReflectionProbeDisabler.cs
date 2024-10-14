using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(ReflectionProbe))]
    public class RealtimeReflectionProbeDisabler : MonoBehaviour
    {
        private ReflectionProbe _reflectionProbe;


        private void Awake()
        {
            _reflectionProbe = GetComponent<ReflectionProbe>();
        }

        private void OnEnable()
        {
            QualitySettings.activeQualityLevelChanged += OnActiveQualityLevelChanged;
            OnActiveQualityLevelChanged(0, QualitySettings.GetQualityLevel());
        }

        private void OnDisable()
        {
            QualitySettings.activeQualityLevelChanged -= OnActiveQualityLevelChanged;
        }

        private void OnActiveQualityLevelChanged(int previousQuality, int currentQuality)
        {
            if (_reflectionProbe.mode != UnityEngine.Rendering.ReflectionProbeMode.Realtime)
            {
                return;
            }

            bool wasEnabled = _reflectionProbe.enabled;
            _reflectionProbe.enabled = QualitySettings.realtimeReflectionProbes;

            if (!wasEnabled && _reflectionProbe.enabled)
            {
                _reflectionProbe.RenderProbe();
            }
        }
    }
}