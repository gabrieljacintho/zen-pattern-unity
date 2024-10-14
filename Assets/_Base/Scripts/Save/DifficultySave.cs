using FireRingStudio.Difficulty;

namespace FireRingStudio.Save
{
    public class DifficultySave : SettingsSave<int>
    {
        public static int DefaultLevel => DifficultyManager.DefaultLevel;
        
        public override string SaveKey => "Difficulty";
        public override int DefaultValue => DefaultLevel;
        public override int CurrentValue => DifficultyManager.CurrentLevel;


        protected override void Load(int value)
        {
            DifficultyManager.CurrentLevel = value;
        }
    }
}