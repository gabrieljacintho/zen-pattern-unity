using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Extensions
{
    public static class GraphicExtensions
    {
        public static void SetAlpha(this Graphic graphic, float value)
        {
            Color color = graphic.color;
            color.a = value;
            graphic.color = color;
        }
    }
}