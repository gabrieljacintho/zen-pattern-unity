using FireRingStudio.FMODIntegration;
using FireRingStudio.Save;
using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    [RequireComponent(typeof(FMODAudioVolumeControl))]
    public class AudioVolumeSlider : SettingsSlider
    {
        private FMODAudioVolumeControl _volumeControl;

        private FMODAudioVolumeControl VolumeControl
        {
            get
            {
                if (_volumeControl == null)
                {
                    _volumeControl = GetComponent<FMODAudioVolumeControl>();
                }

                return _volumeControl;
            }
        }
        protected override float AppliedValue => VolumeControl.Volume;
        protected override float DefaultValue => AudioVolumeSave.DefaultVolume;


        protected override void Apply(float value)
        {
            VolumeControl.Volume = value;
        }
    }
}