using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio.Interaction
{
    public class InteractTextHandler : MonoBehaviour
    {
        [SerializeField] private InteractText _interactText;
        [HideIf("@_interactText != null")]
        [SerializeField] private string _interactTextId;
        [SerializeField] private LocalizedString _description = "Interact";
        [SerializeField] private InputActionReference _input;
        
        private InteractText InteractText => GetInteractText();
        

        public void Show()
        {
            if (InteractText == null)
            {
                Debug.LogError("InteractText not found!", this);
                return;
            }
            
            InteractText.Show(this, _description, _input);
        }

        public void Hide()
        {
            if (InteractText == null)
            {
                return;
            }
            
            InteractText.Hide(this);
        }

        private InteractText GetInteractText()
        {
            if (_interactText != null)
            {
                return _interactText;
            }
            
            _interactText = ComponentID.FindComponentWithID<InteractText>(_interactTextId, true);

            return _interactText;
        }
    }
}