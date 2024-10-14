using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Interaction
{
    public class Opener : InteractorGeneric<OpenableObject>
    {
        [SerializeField] private bool _canClose;
        
        [Header("Description")]
        [SerializeField] private LocalizedString _openDescription = "Open";
        [ShowIf("_canClose")]
        [SerializeField] private LocalizedString _closeDescription = "Close";
        
        
        public override bool TryInteractWith(OpenableObject openableObject)
        {
            if (!openableObject.IsOpen)
            {
                return openableObject.TryOpen(transform.position);
            }
            
            return _canClose && openableObject.TryClose();
        }

        public override bool CanInteractWith(OpenableObject openableObject)
        {
            if (!openableObject.Interactable)
            {
                return false;
            }

            return !openableObject.IsOpen || _canClose;
        }

        protected override string GetInteractionDescription(OpenableObject openableObject)
        {
            if (!openableObject.Interactable)
            {
                return null;
            }

            if (!openableObject.IsOpen)
            {
                return _openDescription;
            }

            return _canClose ? _closeDescription : null;
        }
    }
}