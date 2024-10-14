using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class AudioVolumeSave : SettingsSave<float>
    {
        [SerializeField] private string _vcaName;

        private VCA _vca;

        public const float DefaultVolume = 0.5f;
        
        public override string SaveKey => _vcaName;
        public override float DefaultValue => DefaultVolume;
        public override float CurrentValue => GetCurrentValue();

        public VCA VCA
        {
            get
            {
                if (!_vca.isValid())
                {
                    _vca = RuntimeManager.GetVCA("vca:/" + _vcaName);
                }

                return _vca;
            }
        }


        protected override void Load(float value)
        {
            if (_vcaName == null || !VCA.isValid())
            {
                return;
            }
            
            VCA.setVolume(value);
        }

        private float GetCurrentValue()
        {
            if (_vcaName == null || !VCA.isValid())
            {
                return 0f;
            }

            return VCA.getVolume(out float volume) == RESULT.OK ? volume : 0f;
        }
    }
}