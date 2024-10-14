using System;

namespace FireRingStudio.Operations
{
    [Serializable]
    public class BoolOrOperation : Operation<bool>
    {
        public BoolOrOperation(bool defaultValue) : base(defaultValue) { }
        
        protected override bool GetResult(bool valueA, bool valueB)
        {
            return valueA || valueB;
        }
    }
}