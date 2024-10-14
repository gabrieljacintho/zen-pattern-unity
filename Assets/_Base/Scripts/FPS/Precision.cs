using System;

namespace FireRingStudio.FPS
{
    [Serializable]
    public struct Precision
    {
        public float Radius;
        public CrosshairData Crosshair;

        
        public Precision(float radius, CrosshairData crosshair = null)
        {
            Crosshair = crosshair;
            Radius = radius;
        }
    }
}