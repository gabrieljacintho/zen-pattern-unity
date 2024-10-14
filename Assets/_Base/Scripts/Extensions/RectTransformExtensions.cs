using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class RectTransformExtensions
    {
        public static void SetRect(this RectTransform rectTransform, Rect rect)
        {
            rectTransform.anchoredPosition = rect.position;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
        }
        
        public static void SetSizeWithCurrentAnchors(this RectTransform rectTransform, float horizontal, float vertical)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, horizontal);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vertical);
        }

        public static void SetSizeWithCurrentAnchors(this RectTransform rectTransform, Vector2 size)
        {
            rectTransform.SetSizeWithCurrentAnchors(size.x, size.y);
        }
    }
}