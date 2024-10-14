using FireRingStudio.Cache;
using FireRingStudio.Inventory;
using I2.Loc;
using UnityEngine;

namespace FireRingStudio.Interaction
{
    public class LockInteractor : InteractorGeneric<Lock>
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private Opener _opener;
        
        [Header("Description")]
        [SerializeField] protected LocalizedString _unlockDescription = "Unlock";
        [SerializeField] protected LocalizedString _lockedDescription = "Locked";
        
        
        public override bool TryInteractWith(Lock @lock)
        {
            if (!@lock.TryUnlock(_inventory))
            {
                return false;
            }

            if (_opener != null && @lock.OpenableObject != null)
            {
                _opener.TryInteractWith(@lock.OpenableObject);
            }
            
            return true;
        }

        protected override string GetInteractionDescription(Lock @lock)
        {
            if (@lock.IsUnlocked)
            {
                return null;
            }

            return CanInteractWith(@lock) ? _unlockDescription : _lockedDescription;
        }

        public override bool CanInteract()
        {
            return base.CanInteract() && _inventory != null;
        }
        
        public override bool CanInteractWith(Lock @lock)
        {
            bool value = base.CanInteractWith(@lock);

            if (@lock.IsUnlocked)
            {
                return value;
            }

            return value && _inventory.HasItem(@lock.KeyData);
        }
    }
}