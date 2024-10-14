using System.Collections.Generic;
using System.Linq;
using FireRingStudio.Extensions;
using FireRingStudio.FPS;
using FireRingStudio.FPS.Equipment;
using FireRingStudio.Physics;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Interaction
{
    public abstract class InteractorGeneric<T> : MonoBehaviour, IInteractor where T : InteractableBase
    {
        [SerializeField] private InputActionReference _interactInput;
        [SerializeField] protected EquipmentSwapper _equipmentSwapper;
        [SerializeField] private string _interactTextID;
        
        [Header("Audios")]
        [SerializeField] private EventReference _interactSFX;

        private List<T> _targetInteractables = new List<T>();
        
        private Detector[] _detectors;
        private InteractText _interactText;

        private float _lastInteractionTime;
        private string _lastDescription;

        public virtual bool IsInteracting => Time.time - _lastInteractionTime < 0.1f;
        public List<T> TargetInteractables
        {
            get => _targetInteractables;
            private set => _targetInteractables = value;
        }

        [Space]
        public UnityEvent<List<T>> OnInteract;
        

        protected virtual void Awake()
        {
            _detectors = GetComponentsInChildren<Detector>();
            _interactText = ComponentID.FindComponentWithID<InteractText>(_interactTextID, true);
        }

        protected virtual void OnEnable()
        {
            if (_interactInput != null)
            {
                _interactInput.action.performed += TryInteract;
            }
        }
        
        protected virtual void FixedUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }
            
            UpdateTargetInteractables();
            UpdateInteractText();
        }

        protected virtual void OnDisable()
        {
            if (_interactInput != null)
            {
                _interactInput.action.performed -= TryInteract;
                
                if (_interactText != null)
                {
                    _interactText.Hide(this);
                }
            }
        }
        
        public virtual bool TryInteract()
        {
            if (!CanInteract())
            {
                return false;
            }

            bool interacted = false;
            foreach (T interactable in TargetInteractables)
            {
                if (interactable == null || !CanInteractWith(interactable) || !TryInteractWith(interactable))
                {
                    continue;
                }

                if (!interacted && !_interactSFX.IsNull)
                {
                    _interactSFX.Play(interactable.transform.position);
                }

                interacted = true;
            }

            OnInteract?.Invoke(TargetInteractables);
            
            _targetInteractables.Clear();

            _lastInteractionTime = Time.time;

            return true;
        }
        
        public abstract bool TryInteractWith(T interactable);
        
        protected virtual void TryInteract(InputAction.CallbackContext context)
        {
            if (GameManager.InGame)
            {
                TryInteract();
            }
        }

        private void UpdateTargetInteractables()
        {
            Equipment currentEquipment = _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;
            if (currentEquipment != null && currentEquipment.InUse)
            {
                TargetInteractables.Clear();
            }
            else
            {
                TargetInteractables = GetInteractables();
            }
        }
        
        private void UpdateInteractText()
        {
            if (_interactText == null)
            {
                return;
            }

            T firstTargetInteractable = null;
            if (TargetInteractables != null)
            {
                firstTargetInteractable = TargetInteractables.Find(CanInteractWith);
                if (firstTargetInteractable == null && TargetInteractables.Count > 0)
                {
                    firstTargetInteractable = TargetInteractables[0];
                }
            }
            
            string description = null;
            InputActionReference input = null;
            if (firstTargetInteractable != null)
            {
                description = GetInteractionDescription(firstTargetInteractable);
                
                if (_interactInput != null && CanInteractWith(firstTargetInteractable))
                {
                    input = _interactInput;
                }
            }

            if (_lastDescription == description && _interactText.CurrentSourceComponent != null)
            {
                return;
            }

            if (string.IsNullOrEmpty(description))
            {
                _interactText.Hide(this);
            }
            else
            {
                _interactText.Show(this, description, input);
            }

            _lastDescription = description;
        }
        
        #region Getters/Setters
        
        protected virtual List<T> GetInteractables()
        {
            if (_detectors == null || _detectors.Length == 0)
            {
                return new List<T>();
            }
            
            List<T> values = new List<T>();
            List<T> closestValues = new List<T>();

            foreach (Detector detector in _detectors)
            {
                List<Collider> colliders = detector.GetCollidersOrderByDistance();

                foreach (Collider other in colliders)
                {
                    T[] interactables = other.transform.GetComponentsInChildren<T>();

                    foreach (T interactable in interactables)
                    {
                        if (CanInteractWith(interactable) && !values.Contains(interactable))
                        {
                            values.Add(interactable);
                        }
                    }

                    if (closestValues.Count == 0)
                    {
                        closestValues.AddRange(interactables);
                    }
                }
            }

            return values.Count > 0 ? values : closestValues;
        }
        
        protected virtual string GetInteractionDescription(T interactable)
        {
            return interactable.Description;
        }
        
        #endregion
        
        public virtual bool CanInteract()
        {
            if (!isActiveAndEnabled || TargetInteractables == null || TargetInteractables.Count == 0)
            {
                return false;
            }

            if (_interactText != null && _interactText.CurrentSourceComponent != this)
            {
                return false;
            }

            return true;
        }

        public virtual bool CanInteractWith(T interactable)
        {
            return interactable.Interactable;
        }
    }
}