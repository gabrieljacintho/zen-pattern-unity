using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FireRingStudio.PostProcess
{
    public class SaturationHandler : RangeValueHandler
    {
        [SerializeField] private PostProcessProfile _profile;

        private ColorGrading _colorGrading;

        private ColorGrading ColorGrading => GetColorGrading();


        protected override void Awake()
        {
            base.Awake();
            if (ColorGrading != null)
            {
                ColorGrading.active = true;
                ColorGrading.enabled.value = true;
                ColorGrading.saturation.overrideState = true;
            }
        }

        protected override float GetRangeValue()
        {
            return ColorGrading != null && ColorGrading.saturation != null ? ColorGrading.saturation.value : 0f;
        }

        protected override void SetRangeValue(float value)
        {
            if (ColorGrading != null && ColorGrading.saturation != null)
            {
                ColorGrading.saturation.value = value;
            }
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