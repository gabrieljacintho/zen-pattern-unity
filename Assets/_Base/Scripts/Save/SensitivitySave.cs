using FireRingStudio.Movement;

namespace FireRingStudio.Save
{
    public class SensitivitySave : SettingsSave<float>
    {
        public const float DefaultScale = 0.55f;
        
        public override string SaveKey => "Sensitivity";
        public override float DefaultValue => DefaultScale;
        public override float CurrentValue => PlayerLook.SensitivityScale;


        protected override void Load(float value)
        {
            PlayerLook.SensitivityScale = value;
        }
    }
}