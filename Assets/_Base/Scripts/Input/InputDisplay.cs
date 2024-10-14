using FireRingStudio.Extensions;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FireRingStudio.Input
{
    public class InputDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("_input")]
        [SerializeField] private InputActionReference _inputReference;
        [ShowIf("@_inputReference == null")]
        [SerializeField] private InputAction _input;
        [SerializeField] private bool _useCurrentControlScheme = true;
        [HideIf("@_useCurrentControlScheme")]
        [SerializeField] private ControlScheme _controlScheme;
        [SerializeField] private bool _useBindingIndex;
        [HideIf("@!_useBindingIndex")]
        [SerializeField] private int _bindingIndex;
        
        [Header("UI Elements")]
        [FormerlySerializedAs("_nameText")]
        [FormerlySerializedAs("_inputText")]
        [SerializeField] private TextMeshProUGUI _inputNameText;
        [FormerlySerializedAs("_iconImage")]
        [SerializeField] private Image _bindingImage;
        [FormerlySerializedAs("_bindingText")]
        [SerializeField] private TextMeshProUGUI _bindingDisplayText;

        public InputAction InputAction => _inputReference != null ? _inputReference.action : _input;

        
        private void OnEnable()
        {
            BindingsManager.BindingsUpdated += UpdateInputIcon;
            
            if (_inputNameText != null)
            {
                LocalizationManager.OnLocalizeEvent += UpdateInputName;
                UpdateInputName();
            }

            if (_useCurrentControlScheme)
            {
                InputManager.ControlSchemeChanged += UpdateInputIcon;
            }

            UpdateInputIcon();
        }
        
        private void OnDisable()
        {
            BindingsManager.BindingsUpdated -= UpdateInputIcon;
            
            if (_inputNameText != null)
            {
                LocalizationManager.OnLocalizeEvent -= UpdateInputName;
            }

            if (_useCurrentControlScheme)
            {
                InputManager.ControlSchemeChanged -= UpdateInputIcon;
            }
        }

        public void SetInput(InputAction value)
        {
            _inputReference = null;
            _input = value;
            UpdateInputName();
            UpdateInputIcon();
        }

        private void UpdateInputName()
        {
            if (_inputNameText != null)
            {
                _inputNameText.text = InputAction != null ? InputAction.GetNameLocalized() : string.Empty;
                _inputNameText.gameObject.SetActive(!string.IsNullOrEmpty(_inputNameText.text));
            }
        }
        
        private void UpdateInputIcon(ControlScheme controlScheme)
        {
            UpdateBindingImage(controlScheme);
            UpdateBindingDisplayName(controlScheme);
        }

        private void UpdateBindingImage(ControlScheme controlScheme)
        {
            if (_bindingImage == null)
            {
                return;
            }

            if (InputAction == null)
            {
                _bindingImage.gameObject.SetActive(false);
                return;
            }

            if (_useBindingIndex)
            {
                _bindingImage.sprite = InputAction.GetIcon(_bindingIndex, controlScheme);
            }
            else
            {
                _bindingImage.sprite = InputAction.GetIcon(controlScheme);
            }

            _bindingImage.gameObject.SetActive(_bindingImage.sprite != null);
        }

        private void UpdateBindingDisplayName(ControlScheme controlScheme)
        {
            if (_bindingDisplayText == null)
            {
                return;
            }

            if (InputAction == null)
            {
                _bindingDisplayText.gameObject.SetActive(false);
                return;
            }

            if (_bindingImage == null || !_bindingImage.gameObject.activeSelf)
            {
                if (_useBindingIndex)
                {
                    _bindingDisplayText.text = InputAction.GetDisplayName(_bindingIndex, controlScheme);
                }
                else
                {
                    _bindingDisplayText.text = InputAction.GetDisplayName(controlScheme);
                }
            }
            else
            {
                _bindingDisplayText.text = null;
            }

            _bindingDisplayText.gameObject.SetActive(!string.IsNullOrEmpty(_bindingDisplayText.text));
        }

        private void UpdateInputIcon()
        {
            if (_useCurrentControlScheme)
            {
                UpdateInputIcon(InputManager.CurrentControlScheme);
            }
            else
            {
                UpdateInputIcon(_controlScheme);
            }
        }
    }
}