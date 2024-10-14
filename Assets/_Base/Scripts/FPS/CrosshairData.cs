using UnityEngine;

namespace FireRingStudio.FPS
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Crosshair Data", fileName = "New Crosshair Data")]
    public class CrosshairData : ScriptableObject
    {
        [Header("Left Image")]
        public Sprite leftSprite;
        public Rect leftRect;

        [Header("Right Image")]
        public Sprite rightSprite;
        public Rect rightRect;

        [Header("Center Image")]
        public Sprite centerSprite;
        public Rect centerRect;

        [Header("Top Image")]
        public Sprite topSprite;
        public Rect topRect;

        [Header("Bottom Image")]
        public Sprite bottomSprite;
        public Rect bottomRect;
    }
}
