using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Input;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI.Options
{
    public class SelectionOptionsManager : MonoBehaviour, ICancelable
    {
        [Tooltip("If null the component first child or self Transform is used.")]
        [SerializeField] private Transform _contentRoot;
        [Tooltip("If null the content root is used.")]
        [SerializeField] private Transform _optionButtonsPivot;
        [SerializeField] private GameObject _optionButtonPrefab;
        [Tooltip("If null the option buttons pivot is used.")]
        [SerializeField] private Transform _optionButtonsParent;
        [SerializeField] private int _cancelPriority = 1;

        private List<SelectionOptionButton> _optionButtons;
        private SelectableOptions _lastSelectedOptions;

        public int Priority => _cancelPriority;
        private Transform ContentRoot => _contentRoot != null ? _contentRoot : transform.childCount > 0 ? transform.GetChild(0) : transform;
        private Transform OptionButtonsPivot => _optionButtonsPivot != null ? _optionButtonsPivot : ContentRoot;
        private Transform OptionButtonsParent => _optionButtonsParent != null ? _optionButtonsParent : OptionButtonsPivot;


        private void Awake()
        {
            _optionButtons = OptionButtonsParent.GetComponentsInChildren<SelectionOptionButton>(true).ToList();
        }

        private void OnEnable()
        {
            _optionButtons.SetOnClickListener(OnClickOptionButton, true);
        }
        
        private void OnDisable()
        {
            _optionButtons.SetOnClickListener(OnClickOptionButton, false);
            Cancel();
        }

        public void OnClickSelectableOptions(SelectableOptions selectable)
        {
            if (_lastSelectedOptions == selectable && ContentRoot.gameObject.activeSelf)
            {
                return;
            }
            
            if (_lastSelectedOptions != null && _lastSelectedOptions.IsExecuting)
            {
                return;
            }
            
            if (!selectable.HasAnyOptionAvailable())
            {
                Cancel();
                return;
            }
            
            InitializeOptionButtons(selectable);
            OrderOptionButtons();
            FixOptionButtonsNavigation();

            OptionButtonsPivot.position = selectable.transform.position;
            ContentRoot.gameObject.SetActive(true);
            
            _lastSelectedOptions = selectable;
        }
        
        public void Cancel()
        {
            ContentRoot.gameObject.SetActive(false);
        }
        
        public bool CanCancel()
        {
            return ContentRoot.gameObject.activeSelf || (_lastSelectedOptions != null && _lastSelectedOptions.IsExecuting);
        }
        
        private void InitializeOptionButtons(SelectableOptions selectable)
        {
            ResetOptionButtons();
            
            foreach (SelectableOption option in selectable.Options)
            {
                if (!option.IsAvailable())
                {
                    continue;
                }
                
                SelectionOptionButton optionButton = _optionButtons.Find(button => !button.gameObject.activeSelf);
                if (optionButton == null)
                {
                    optionButton = CreateOptionButton();
                }
                
                if (optionButton == null)
                {
                    continue;
                }

                optionButton.Initialize(option);
                optionButton.gameObject.SetActive(true);
            }
        }
        
        private void ResetOptionButtons()
        {
            _optionButtons.ForEach(button => button.gameObject.SetActive(false));
        }

        private SelectionOptionButton CreateOptionButton()
        {
            if (_optionButtonPrefab == null)
            {
                return null;
            }
            
            SelectionOptionButton optionButton = _optionButtonPrefab.Get(OptionButtonsParent).GetComponent<SelectionOptionButton>();
            if (optionButton != null)
            {
                optionButton.SetOnClickListener(OnClickOptionButton, true);
                _optionButtons.Add(optionButton);
            }

            return optionButton;
        }

        private void OrderOptionButtons()
        {
            _optionButtons.Sort((buttonA, buttonB) =>
            {
                int orderA = buttonA.Data != null ? buttonA.Data.Order : int.MinValue;
                int orderB = buttonB.Data != null ? buttonB.Data.Order : int.MinValue;

                if (orderA == orderB)
                {
                    return 0;
                }

                return orderA > orderB ? 1 : 0;
            });

            for (int i = 0; i < _optionButtons.Count; i++)
            {
                _optionButtons[i].transform.SetSiblingIndex(i);
            }
        }

        private void FixOptionButtonsNavigation()
        {
            for (int i = 0; i < _optionButtons.Count; i++)
            {
                Button currentButton = _optionButtons[i].Button;
                Button previousButton = i == 0 ? _optionButtons[^1].Button : _optionButtons[i - 1].Button;
                Button nextButton = i == _optionButtons.Count - 1 ? _optionButtons[0].Button : _optionButtons[i + 1].Button;
                
                Navigation navigation = currentButton.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = previousButton;
                navigation.selectOnDown = nextButton;
                currentButton.navigation = navigation;
            }
        }
        
        private void OnClickOptionButton(SelectionOptionButton optionButton)
        {
            Cancel();
        }
    }
}