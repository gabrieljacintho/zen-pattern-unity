namespace FireRingStudio.Difficulty
{
    public class DifficultyVariableHandler : VariableHandler
    {
        protected override int CurrentIndex => DifficultyManager.CurrentLevel;


        protected override void OnEnable()
        {
            base.OnEnable();
            DifficultyManager.LevelChanged += UpdateVariable;
        }
        
        private void OnDisable()
        {
            DifficultyManager.LevelChanged -= UpdateVariable;
        }
    }
}