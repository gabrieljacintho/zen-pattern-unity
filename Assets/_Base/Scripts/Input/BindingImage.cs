using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Input
{
    [RequireComponent(typeof(Image))]
    public class BindingImage : MonoBehaviour
    {
        [SerializeField] private string _bindingPath;
        [SerializeField] private bool _useCurrentControlScheme = true;
        [HideIf("@_useCurrentControlScheme")]
        [SerializeField] private ControlScheme _controlScheme;

        private Image _image;
        

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            BindingsManager.BindingsUpdated += UpdateImage;
            UpdateImage();
        }

        private void OnDisable()
        {
            BindingsManager.BindingsUpdated -= UpdateImage;
        }

        private void UpdateImage()
        {
            ControlScheme controlScheme = _useCurrentControlScheme ? InputManager.CurrentControlScheme : _controlScheme;
            _image.sprite = BindingsManager.Instance != null ? BindingsManager.Instance.GetIcon(_bindingPath, controlScheme) : null;
            _image.enabled = _image.sprite != null;
        }
    }
}