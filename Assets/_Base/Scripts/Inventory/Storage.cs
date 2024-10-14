using FireRingStudio.Interaction;
using FireRingStudio.Inventory.UI;
using FireRingStudio.UI;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    public class Storage : OpenableObject
    {
        [Header("Storage")]
        [SerializeField, ES3NonSerializable] private InventoryData _inventory;
        [SerializeField, ES3NonSerializable] private string _storageUIId = "storage";
        [SerializeField, ES3NonSerializable] private string _inventoryDisplayId = "storage";

        private InventoryDisplay _inventoryDisplay;
        
        public override bool Interactable => base.Interactable && _inventory != null && !string.IsNullOrEmpty(_storageUIId)
                                             && UIManager.Instance != null && InventoryDisplay != null;
        private InventoryDisplay InventoryDisplay
        {
            get
            {
                if (_inventoryDisplay == null)
                {
                    _inventoryDisplay = ComponentID.FindComponentWithID<InventoryDisplay>(_inventoryDisplayId, true);
                }

                return _inventoryDisplay;
            }
        }
        

        public override bool TryOpen(Vector3 agentPosition)
        {
            if (!Interactable || !base.TryOpen(agentPosition))
            {
                return false;
            }

            InventoryDisplay.Data = _inventory;

            UIManager.Instance.OpenUI(_storageUIId);
            UIManager.Instance.OnUIIdChanged.AddListener(OnUIIdChanged);
            UIManager.Instance.OnClosed.AddListener(OnCloseInventoryUI);

            GameManager.CurrentGameState = GameState.Interaction;

            return true;
        }

        private void OnUIIdChanged(string id)
        {
            if (id != _storageUIId)
            {
                OnCloseInventoryUI();
            }
        }

        private void OnCloseInventoryUI()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.OnUIIdChanged.RemoveListener(OnUIIdChanged);
                UIManager.Instance.OnClosed.RemoveListener(OnCloseInventoryUI);
            }
            
            ForceClose();
        }
    }
}