using UnityEngine;

namespace FireRingStudio.Random
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialColorRandomizer : Randomizer<ColorRange>
    {
        private Material _material;

        
        protected override void Awake()
        {
            _material = GetComponent<Renderer>().material;
            
            base.Awake();
        }

        protected override void ApplyValue(ColorRange value)
        {
            _material.color = value.GetRandomValue();
        }
    }
}