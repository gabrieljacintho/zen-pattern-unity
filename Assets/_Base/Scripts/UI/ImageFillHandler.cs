using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Image))]
    public class ImageFillHandler : RangeValueHandler
    {
        private Image _image;

        private Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = GetComponent<Image>();
                }

                return _image;
            }
        }


        protected override float GetRangeValue()
        {
            return Image != null ? Image.fillAmount : 0f;
        }

        protected override void SetRangeValue(float value)
        {
            if (Image != null)
            {
                Image.fillAmount = value;
            }
        }
    }
}