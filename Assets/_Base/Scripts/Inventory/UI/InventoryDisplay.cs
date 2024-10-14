using System.Collections.Generic;
using FireRingStudio.Extensions;
using TMPro;
using UnityEngine;

namespace FireRingStudio.Inventory.UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private InventoryData _data;
        [SerializeField] private TextMeshProUGUI _displayNameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private GameObject _slotPanelPrefab;
        [Tooltip("If null the component Transform is used.")]
        [SerializeField] private Transform _slotPanelsRoot;

        private List<InventorySlotDisplay> _slotDisplays;

        public InventoryData Data
        {
            get => _data;
            set
            {
                if (_data != value)
                {
                    _data = value;
                    UpdatePanel();
                }
            }
        }
        public List<InventorySlotDisplay> SlotDisplays
        {
            get
            {
                if (_slotDisplays == null)
                {
                    _slotDisplays = SlotPanelsRoot.GetComponentsInChildren<InventorySlotDisplay>(true).ToList();
                }

                return _slotDisplays;
            }
        }
        private Transform SlotPanelsRoot => _slotPanelsRoot != null ? _slotPanelsRoot : transform;


        private void OnEnable()
        {
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            UpdateDisplayNameText();
            UpdateDescriptionText();
            UpdateSlotPanels();
        }

        private void UpdateDisplayNameText()
        {
            if (_displayNameText != null)
            {
                _displayNameText.text = _data != null ? _data.DisplayName : null;
                _displayNameText.gameObject.SetActive(!string.IsNullOrEmpty(_displayNameText.text));
            }
        }

        private void UpdateDescriptionText()
        {
            if (_descriptionText != null)
            {
                _descriptionText.text = _data != null ? _data.Description : null;
                _descriptionText.gameObject.SetActive(!string.IsNullOrEmpty(_descriptionText.text));
            }
        }

        private void UpdateSlotPanels()
        {
            if (_data == null)
            {
                SlotDisplays.ForEach(slotDisplay => slotDisplay.gameObject.SetActive(false));
                return;
            }
            
            foreach (InventorySlot slot in _data.Slots)
            {
                InventorySlotDisplay slotDisplay = GetSlotPanel(slot);
                if (slotDisplay == null)
                {
                    continue;
                }
                
                if (!SlotDisplays.Contains(slotDisplay))
                {
                    SlotDisplays.Add(slotDisplay);
                }

                slotDisplay.Initialize(slot);
                slotDisplay.gameObject.SetActive(true);
            }

            List<InventorySlotDisplay> invalidSlotDisplays = FindAllInvalidSlotPanels();
            invalidSlotDisplays.ForEach(slotPanel => slotPanel.gameObject.SetActive(false));
        }

        private InventorySlotDisplay GetSlotPanel(InventorySlot slot)
        {
            InventorySlotDisplay slotDisplay = SlotDisplays.Find(slotPanel => slotPanel.Slot == slot);
            if (slotDisplay != null)
            {
                return slotDisplay;
            }

            List<InventorySlotDisplay> invalidSlotDisplays = FindAllInvalidSlotPanels();
            if (invalidSlotDisplays.Count > 0)
            {
                return invalidSlotDisplays[0];
            }

            if (_slotPanelPrefab != null)
            {
                slotDisplay = _slotPanelPrefab.Get(SlotPanelsRoot).GetComponent<InventorySlotDisplay>();
            }

            return slotDisplay;
        }

        private List<InventorySlotDisplay> FindAllInvalidSlotPanels()
        {
            return SlotDisplays.FindAll(slotPanel => slotPanel.Slot == null || (_data != null && !_data.Slots.Contains(slotPanel.Slot)));
        }
    }
}