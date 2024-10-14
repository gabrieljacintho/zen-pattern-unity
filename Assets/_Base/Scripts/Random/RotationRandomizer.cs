using UnityEngine;

namespace FireRingStudio.Random
{
    public class RotationRandomizer : Randomizer<Vector3Range>
    {
        [Space]
        [SerializeField] private Space _space = Space.World;
        
        
        protected override void ApplyValue(Vector3Range value)
        {
            Quaternion randomValue = Quaternion.Euler(value.GetRandomValue());
            if (_space == Space.Self)
            {
                transform.localRotation = randomValue;
            }
            else
            {
                transform.rotation = randomValue;
            }
        }
    }
}