using UnityEngine;

namespace FireRingStudio.Random
{
    public class TransformRandomizer : Randomizer<Transform>
    {
        [Space]
        [SerializeField] private bool _applyPosition = true;
        [SerializeField] private bool _applyRotation = true;
        [SerializeField] private bool _applyScale = true;
        
        
        protected override void ApplyValue(Transform value)
        {
            if (_applyPosition)
            {
                transform.position = value.position;
            }

            if (_applyRotation)
            {
                transform.rotation = value.rotation;
            }

            if (_applyScale)
            {
                transform.localScale = value.localScale;
            }
        }
    }
}