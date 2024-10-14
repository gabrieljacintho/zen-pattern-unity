using FireRingStudio.Cache;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace FireRingStudio.Inventory.UI
{
    public class SelectedItemDataDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _contentObject;
        [SerializeField] private TextMeshProUGUI _displayNameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Transform _requiredParent;
        [SerializeField] private LocalizedString _emptyText;
        
        private InventorySlot _selectedInventorySlot;
        

        private void OnEnable()
        {
            LocalizationManager.OnLocalizeEvent += UpdateDisplay;
            EventManager.MainSelectedObjectChanged += UpdateDisplay;
            UpdateDisplay();
        }

        private void OnDisable()
        {
            LocalizationManager.OnLocalizeEvent -= UpdateDisplay;
            EventManager.MainSelectedObjectChanged -= UpdateDisplay;
            
            if (_selectedInventorySlot != null)
            {
                _selectedInventorySlot.Changed -= UpdateDisplay;
                _selectedInventorySlot = null;
            }
        }

        private void UpdateDisplay(GameObject selectedObject)
        {
            if (selectedObject == null || (_requiredParent != null && !selectedObject.transform.IsChildOf(_requiredParent)))
            {
                return;
            }

            InventorySlot inventorySlot = ComponentCacheManager.GetComponent<InventorySlotDisplay>(selectedObject)?.Slot;
            if (inventorySlot == null)
            {
                return;
            }
            
            if (_selectedInventorySlot != null)
            {
                _selectedInventorySlot.Changed -= UpdateDisplay;
            }

            _selectedInventorySlot = inventorySlot;
            _selectedInventorySlot.Changed += UpdateDisplay;
            
            UpdateDisplay();
        }
        
        private void UpdateDisplay()
        {
            ItemData itemData = _selectedInventorySlot != null && !_selectedInventorySlot.IsEmpty ? _selectedInventorySlot.ItemData : null;
            UpdateDisplay(itemData);
        }
        
        private void UpdateDisplay(ItemData itemData)
        {
            if (_displayNameText != null)
            {
                _displayNameText.text = itemData != null ? itemData.DisplayName : _emptyText;
                _displayNameText.gameObject.SetActive(!string.IsNullOrEmpty(_displayNameText.text));
            }
            
            if (_descriptionText != null)
            {
                _descriptionText.text = itemData != null ? itemData.Description : null;
                _descriptionText.gameObject.SetActive(!string.IsNullOrEmpty(_descriptionText.text));
            }

            if (_contentObject != null)
            {
                bool hasActiveContent = _displayNameText != null && _displayNameText.gameObject.activeSelf;
                hasActiveContent |= _descriptionText != null && _descriptionText.gameObject.activeSelf;
                _contentObject.SetActive(hasActiveContent);
            }
        }
    }
}