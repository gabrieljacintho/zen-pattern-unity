using FireRingStudio.Extensions;
using FireRingStudio.Inventory;
using FireRingStudio.Inventory.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    public enum SelectableState
    {
        Normal,
        Selected,
        SelectedSlot,
        Disabled
    }
    
    public abstract class SelectableComponent : MonoBehaviour
    {
        [Tooltip("If null the component GameObject is used.")]
        [SerializeField] private GameObject _selectableObject;
        [SerializeField] private InventorySlotDisplay _inventorySlotDisplay;
        [SerializeField] private bool _alwaysUpdate;
        
        private Selectable _selectable;
        
        public GameObject SelectableObject => _selectableObject != null ? _selectableObject : gameObject;
        public Selectable Selectable
        {
            get
            {
                if (_selectable == null)
                {
                    _selectable = SelectableObject.GetComponentInParent<Selectable>();
                }
                
                return _selectable;
            }
        }
        public SelectableState CurrentState => GetCurrentState();
        
        
        protected virtual void OnEnable()
        {
            EventManager.MainSelectedObjectChanged += UpdateComponent;
            UpdateComponent();
            this.DoOnNextFrame(UpdateComponent);
        }

        protected virtual void Update()
        {
            if (_alwaysUpdate)
            {
                UpdateComponent();
            }
        }

        protected virtual void OnDisable()
        {
            EventManager.MainSelectedObjectChanged -= UpdateComponent;
        }
        
        public abstract void UpdateComponent();

        private void UpdateComponent(GameObject selectedObject) => UpdateComponent();
        
        private void UpdateComponent(InventorySlot selectedSlot) => UpdateComponent();

        private SelectableState GetCurrentState()
        {
            if (Selectable == null || !Selectable.interactable)
            {
                return SelectableState.Disabled;
            }

            if (EventManager.MainSelectedObject == SelectableObject)
            {
                return SelectableState.Selected;
            }

            if (_inventorySlotDisplay != null && _inventorySlotDisplay.Slot != null && _inventorySlotDisplay.Slot.IsSelected)
            {
                return SelectableState.SelectedSlot;
            }
            
            return SelectableState.Normal;
        }
    }
}