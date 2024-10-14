namespace FireRingStudio.Random
{
    public class ScaleRandomizer : Randomizer<Vector3Range>
    {
        protected override void ApplyValue(Vector3Range value)
        {
            transform.localScale = value.GetRandomValue();
        }
    }
}