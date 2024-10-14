using FireRingStudio.Movement;
using FireRingStudio.Save;

namespace FireRingStudio.UI.Settings
{
    public class SensitivitySlider : SettingsSlider
    {
        protected override float AppliedValue => PlayerLook.SensitivityScale;
        protected override float DefaultValue => SensitivitySave.DefaultScale;


        protected override void Apply(float value)
        {
            PlayerLook.SensitivityScale = value;
        }
    }
}