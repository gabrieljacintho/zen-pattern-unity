using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI.Options
{
    public class SelectionOptionButton : ButtonComponent
    {
        [SerializeField] private TextMeshProUGUI _displayNameText;
        [SerializeField] private Image _iconImage;
        
        private SelectableOption _selectableOption;

        public SelectableOption SelectableOption => _selectableOption;
        public GameObject SelectableObject => _selectableOption != null ? _selectableOption.gameObject : null;
        public SelectableOptionData Data => _selectableOption != null ? _selectableOption.Data : null;
        private bool IsAvailable => _selectableOption != null && _selectableOption.IsAvailable();


        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateButton();
        }

        public void Initialize(SelectableOption selectableOption)
        {
            _selectableOption = selectableOption;
            UpdateButton();
        }
        
        protected override void OnClick()
        {
            if (IsAvailable)
            {
                _selectableOption.Execute();
            }
        }

        private void UpdateButton()
        {
            Button.interactable = IsAvailable;

            if (_displayNameText != null)
            {
                _displayNameText.text = Data != null ? Data.DisplayName : null;
            }

            if (_iconImage != null)
            {
                _iconImage.sprite = Data != null ? Data.Icon : null;
                _iconImage.gameObject.SetActive(_iconImage.sprite != null);
            }
        }
    }
}