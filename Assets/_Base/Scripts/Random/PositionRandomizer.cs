using UnityEngine;

namespace FireRingStudio.Random
{
    public class PositionRandomizer : Randomizer<Vector3Range>
    {
        [Space]
        [SerializeField] private Space _space = Space.World;


        protected override void ApplyValue(Vector3Range value)
        {
            Vector3 randomValue = value.GetRandomValue();
            if (_space == Space.Self)
            {
                transform.localPosition = randomValue;
            }
            else
            {
                transform.position = randomValue;
            }
        }
    }
}