using I2.Loc;
using UnityEngine;

namespace FireRingStudio.Interaction
{
    public class Interactor : InteractorGeneric<Interactable>
    {
        [Header("Description")]
        [SerializeField] protected LocalizedString _interactDescription = "Interact";
        [SerializeField] protected LocalizedString _cannotInteractDescription = "CannotInteract";
        
        
        public override bool TryInteractWith(Interactable interactable)
        {
            return interactable.TryInteract();
        }

        protected override string GetInteractionDescription(Interactable interactable)
        {
            return CanInteractWith(interactable) ? _interactDescription : _cannotInteractDescription;
        }
    }
}