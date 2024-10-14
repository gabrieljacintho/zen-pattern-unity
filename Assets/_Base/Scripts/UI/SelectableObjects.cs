using UnityEngine;

namespace FireRingStudio.UI
{
    public class SelectableObjects : SelectableComponent
    {
        [Header("States")]
        [SerializeField] private GameObject _normalObject;
        [SerializeField] private GameObject _selectedObject;
        [SerializeField] private GameObject _selectedSlotObject;
        [SerializeField] private GameObject _disabledObject;

        public GameObject NormalObject
        {
            get => _normalObject;
            set
            {
                if (_normalObject != value)
                {
                    _normalObject = value;
                    UpdateComponent();
                }
            }
        }
        public GameObject SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (_selectedObject != value)
                {
                    _selectedObject = value;
                    UpdateComponent();
                }
            }
        }
        public GameObject SelectedSlotObject
        {
            get => _selectedSlotObject;
            set
            {
                if (_selectedSlotObject != value)
                {
                    _selectedSlotObject = value;
                    UpdateComponent();
                }
            }
        }
        public GameObject DisabledObject
        {
            get => _disabledObject;
            set
            {
                if (_disabledObject != value)
                {
                    _disabledObject = value;
                    UpdateComponent();
                }
            }
        }
        
        
        public override void UpdateComponent()
        {
            ResetObjects();

            GameObject currentStateObject = GetCurrentStateObject();
            if (currentStateObject != null)
            {
                currentStateObject.SetActive(true);
            }
        }

        private void ResetObjects()
        {
            if (_normalObject != null)
            {
                _normalObject.SetActive(false);
            }

            if (_selectedObject != null)
            {
                _selectedObject.SetActive(false);
            }

            if (_selectedSlotObject != null)
            {
                _selectedSlotObject.SetActive(false);
            }

            if (_disabledObject != null)
            {
                _disabledObject.SetActive(false);
            }
        }
        
        private GameObject GetCurrentStateObject()
        {
            switch (CurrentState)
            {
                case SelectableState.Normal:
                    return _normalObject;
                
                case SelectableState.Selected:
                    return _selectedObject;
                
                case SelectableState.SelectedSlot:
                    return _selectedSlotObject;
                
                case SelectableState.Disabled:
                    return _disabledObject;
                
                default:
                    return null;
            }
        }
    }
}