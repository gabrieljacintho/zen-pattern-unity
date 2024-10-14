using UnityEngine;

namespace FireRingStudio.Save
{
    public class QualitySave : SettingsSave<int>
    {
        public static int DefaultLevel => QualitySettings.names.Length - 1;
        
        public override string SaveKey => "Quality";
        public override int DefaultValue => DefaultLevel;
        public override int CurrentValue => QualitySettings.GetQualityLevel();


        protected override void Load(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }
    }
}