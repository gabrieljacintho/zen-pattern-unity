using System;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Input;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Windows;

namespace FireRingStudio.Interaction
{
    [Serializable]
    public enum StringModification
    {
        None,
        UpperFirstChar,
        LowerFirstChar,
        ToLower,
        ToUpper
    }

    [RequireComponent(typeof(InputDisplay))]
    public class InteractText : MonoBehaviour
    {
        [InfoBox("All GameObject children are disabled when hide is called!", InfoMessageType.Warning)]
        [FormerlySerializedAs("_inputModeText")]
        [SerializeField] private TextMeshProUGUI _interactionText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private LocalizedString _descriptionPrefix;
        [SerializeField] private LocalizedString _descriptionSuffix;
        [SerializeField] private StringModification _descriptionMod;

        private InputDisplay _inputDisplay;
        private Component _currentSourceComponent;
        private InputActionReference _currentInput;

        public Component CurrentSourceComponent => _currentSourceComponent;

        [Space]
        public UnityEvent onShow;
        public UnityEvent onHide;


        private void Awake()
        {
            _inputDisplay = GetComponent<InputDisplay>();
        }

        private void OnEnable()
        {
            LocalizationManager.OnLocalizeEvent += UpdateInteractionText;
            UpdateInteractionText();
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizeEvent -= UpdateInteractionText;
        }

        public void Show(Component sourceComponent, string description, InputActionReference input = null)
        {
            transform.SetActiveChildren(true);

            _currentSourceComponent = sourceComponent;
            _currentInput = input;

            UpdateInteractionText();
            _inputDisplay.SetInput(_currentInput != null ? _currentInput.action : null);
            UpdateDescriptionText(description);

            onShow?.Invoke();
        }

        public void Hide(Component sourceComponent)
        {
            if (_currentSourceComponent == null || _currentSourceComponent != sourceComponent)
            {
                return;
            }
            
            transform.SetActiveChildren(false);

            _currentSourceComponent = null;
            _currentInput = null;
            
            onHide?.Invoke();
        }

        private void UpdateInteractionText()
        {
            if (_interactionText == null)
            {
                return;
            }

            if (_currentInput != null)
            {
                _interactionText.text = _currentInput.GetInteractionLocalized();
                _interactionText.gameObject.SetActive(true);
            }
            else
            {
                _interactionText.gameObject.SetActive(false);
            }
        }

        private void UpdateDescriptionText(string description)
        {
            if (_descriptionText == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                _descriptionText.gameObject.SetActive(false);
                return;
            }

            _descriptionText.gameObject.SetActive(true);

            if (_currentInput == null)
            {
                _descriptionText.text = description;
                return;
            }

            switch (_descriptionMod)
            {
                case StringModification.UpperFirstChar:
                    description = description.ToUpperFirstChar();
                    break;
                
                case StringModification.LowerFirstChar:
                    description = description.ToLowerFirstChar();
                    break;

                case StringModification.ToLower:
                    description = description.ToLower();
                    break;

                case StringModification.ToUpper:
                    description = description.ToUpper();
                    break;
            }

            string prefix = _descriptionPrefix;
            if (!string.IsNullOrEmpty(prefix))
            {
                prefix += " ";
            }

            string suffix = _descriptionSuffix;
            if (!string.IsNullOrEmpty(suffix))
            {
                suffix.Insert(0, " ");
            }

            _descriptionText.text = prefix + description + suffix;
        }
    }
}