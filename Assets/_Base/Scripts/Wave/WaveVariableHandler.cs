namespace FireRingStudio.Wave
{
    public class WaveVariableHandler : VariableHandler
    {
        protected override int CurrentIndex => WaveManager.CurrentWave;


        protected override void OnEnable()
        {
            base.OnEnable();
            WaveManager.WaveChanged += UpdateVariable;
        }

        protected void OnDisable()
        {
            WaveManager.WaveChanged -= UpdateVariable;
        }
    }
}