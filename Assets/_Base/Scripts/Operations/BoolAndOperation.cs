using System;

namespace FireRingStudio.Operations
{
    [Serializable]
    public class BoolAndOperation : Operation<bool>
    {
        public BoolAndOperation(bool defaultValue) : base(defaultValue) { }
        
        protected override bool GetResult(bool valueA, bool valueB)
        {
            return valueA && valueB;
        }
    }
}