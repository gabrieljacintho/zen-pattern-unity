using System;

namespace FireRingStudio.Operations
{
    [Serializable]
    public class FloatMultiplication : Operation<float>
    {
        public FloatMultiplication(float defaultValue) : base(defaultValue) { }
        
        protected override float GetResult(float valueA, float valueB)
        {
            return valueA * valueB;
        }
    }
}