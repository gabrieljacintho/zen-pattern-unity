using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FireRingStudio.UI.Settings
{
    [RequireComponent(typeof(Button))]
    public class ApplySettingsButton : MonoBehaviour
    {
        [FormerlySerializedAs("_settingsApplier")]
        [SerializeField] private SettingsHandler _settingsHandler;
        
        private Button _button;


        private void Awake()
        {
            _button = GetComponent<Button>();

            if (_settingsHandler != null)
            {
                _button.onClick.AddListener(_settingsHandler.ApplyChanges);
            }
        }

        private void OnDisable()
        {
            _button.interactable = false;
        }

        private void Update()
        {
            if (_settingsHandler == null)
            {
                return;
            }
            
            _button.interactable = _settingsHandler.HasChanges();
        }
    }
}