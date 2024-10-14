using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI.Settings
{
    [RequireComponent(typeof(Button))]
    public class ResetSettingsButton : MonoBehaviour
    {
        [SerializeField] private SettingsHandler _settingsHandler;
        
        protected Button _button;


        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            _button.interactable = false;
        }

        protected virtual void Update()
        {
            UpdateButton();
        }

        protected virtual void UpdateButton()
        {
            _button.interactable = IsInteractable();
        }

        protected virtual bool IsInteractable()
        {
            return _settingsHandler != null && !_settingsHandler.IsAllDefault();
        }

        protected virtual void OnClick()
        {
            if (_settingsHandler != null)
            {
                _settingsHandler.ResetSettings();
            }
        }
    }
}