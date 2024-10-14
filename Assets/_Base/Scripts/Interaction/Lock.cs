using FireRingStudio.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Interaction
{
    public class Lock : InteractableBase
    {
        [SerializeField] private bool _isUnlocked;
        [SerializeField, ES3NonSerializable] private ItemData _keyData;
        [SerializeField, ES3NonSerializable] private bool _consumeKeyOnUnlock;
        
        private bool _initialUnlockState;

        public override bool Interactable => base.Interactable && _keyData != null;
        public bool IsUnlocked
        {
            get => _isUnlocked;
            private set => SetIsUnlocked(value);
        }
        public ItemData KeyData => _keyData;
        public OpenableObject OpenableObject { get; set; }
        
        [Space]
        [ES3NonSerializable] public UnityEvent OnUnlock;
        [ES3NonSerializable] public UnityEvent OnLock;


        protected override void Awake()
        {
            base.Awake();
            _initialUnlockState = _isUnlocked;
        }

        public bool TryUnlock(InventoryData inventory)
        {
            if (IsUnlocked)
            {
                return true;
            }
            
            if (!Interactable || !inventory.HasItem(_keyData))
            {
                return false;
            }

            if (_consumeKeyOnUnlock)
            {
                inventory.RemoveItem(_keyData);
            }

            IsUnlocked = true;

            return true;
        }

        public bool TryLock(InventoryData inventory)
        {
            if (!IsUnlocked)
            {
                return true;
            }
            
            if (!Interactable || !inventory.HasItem(_keyData))
            {
                return false;
            }

            IsUnlocked = false;
            
            return true;
        }

        private void SetIsUnlocked(bool value)
        {
            if (_isUnlocked == value)
            {
                return;
            }

            _isUnlocked = value;

            if (_isUnlocked)
            {
                OnUnlock?.Invoke();
            }
            else
            {
                OnLock?.Invoke();
            }
        }

        [HideIf("_isUnlocked")]
        [Button("Unlock")]
        public void ForceUnlock() => IsUnlocked = true;
        
        [ShowIf("_isUnlocked")]
        [Button("Lock")]
        public void ForceLock() => IsUnlocked = false;
        
        public void Restore()
        {
            IsUnlocked = _initialUnlockState;
        }
    }
}