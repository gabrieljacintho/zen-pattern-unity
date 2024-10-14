using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Interaction
{
    public class Interactable : InteractableBase
    {
        [SerializeField, ES3NonSerializable] private LocalizedString _interactDescription = "Interact";
        [SerializeField, ES3NonSerializable] private LocalizedString _cannotInteractDescription = "CannotInteract";

        public override string Description => Interactable ? _interactDescription : _cannotInteractDescription;
        
        [Space]
        [ES3NonSerializable] public UnityEvent OnInteract;
        
        
        public virtual bool TryInteract()
        {
            if (!Interactable)
            {
                return false;
            }
            
            OnInteract?.Invoke();

            return true;
        }
    }
}