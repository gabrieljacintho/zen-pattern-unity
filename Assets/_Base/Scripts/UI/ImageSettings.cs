using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Image)), ExecuteInEditMode]
    public class ImageSettings : MonoBehaviour
    {
        public Image Image { get; private set; }

        public ColorConstant color;


        private void Awake()
        {
            Image = GetComponent<Image>();
            if (Image == null)
                Debug.LogNo(nameof(UnityEngine.UI.Image));
        }

        private void Update()
        {
            if (Image == null)
                return;

            if (color != null)
                Image.color = color.Value;
        }
    }
}