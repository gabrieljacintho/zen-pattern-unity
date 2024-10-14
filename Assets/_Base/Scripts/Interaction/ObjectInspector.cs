using FireRingStudio.Extensions;
using FireRingStudio.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.Interaction
{
    public class ObjectInspector : InteractorGeneric<InteractableBase>
    {
        [Header("Inspector")]
        [SerializeField] private InputActionReference _stopInteractionInput;
        [SerializeField] private Transform _inspectPoint;
        [SerializeField] private string _inspectorUI = "inspect";
        [SerializeField] private float _startInspectDelay;
        [SerializeField] private float _continueGameDelay = 0.1f;

        private InteractableBase _currentInteractable;
        private string _previousUI;

        public override bool IsInteracting => base.IsInteracting || _currentInteractable != null;

        [Space]
        public UnityEvent OnEndInteraction;


        protected override void OnEnable()
        {
            base.OnEnable();

            if (_stopInteractionInput != null)
            {
                _stopInteractionInput.action.performed += StopInteraction;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_stopInteractionInput != null)
            {
                _stopInteractionInput.action.performed -= StopInteraction;
            }

            StopInteraction();
        }

        public override bool TryInteractWith(InteractableBase interactable)
        {
            GameManager.CurrentGameState = GameState.Interaction;

            if (_equipmentSwapper != null)
            {
                _equipmentSwapper.Unequip(true);
            }

            if (UIManager.Instance != null)
            {
                _previousUI = UIManager.Instance.CurrentUI.Id;
                UIManager.Instance.OpenUI(_inspectorUI);
            }

            StopAllCoroutines();

            _currentInteractable = interactable;

            if (_inspectPoint != null)
            {
                void StartInspect()
                {
                    _currentInteractable.StartInspect(_inspectPoint);
                }

                if (_startInspectDelay > 0f)
                {
                    this.DoAfterSeconds(StartInspect, _startInspectDelay);
                }
                else
                {
                    StartInspect();
                }
            }

            return true;
        }

        public override bool CanInteract()
        {
            return base.CanInteract() && _inspectPoint != null;
        }

        public override bool CanInteractWith(InteractableBase interactable)
        {
            return base.CanInteractWith(interactable) && interactable.Inspectable;
        }

        public void StopInteraction()
        {
            if (_currentInteractable == null)
            {
                return;
            }

            StopAllCoroutines();

            if (GameManager.CurrentGameState == GameState.Interaction)
            {
                static void ContinueGame()
                {
                    GameManager.CurrentGameState = GameState.InGame;
                }

                if (_continueGameDelay > 0f)
                {
                    this.DoAfterSeconds(ContinueGame, _continueGameDelay);
                }
                else
                {
                    ContinueGame();
                }

                if (_equipmentSwapper != null)
                {
                    _equipmentSwapper.UpdateEquipment();
                }
            }

            if (UIManager.Instance != null)
            {
                if (string.IsNullOrEmpty(_previousUI))
                {
                    UIManager.Instance.CloseUI();
                }
                else
                {
                    UIManager.Instance.OpenUI(_previousUI);
                }
            }

            _currentInteractable.CancelInspect();
            _currentInteractable = null;

            OnEndInteraction?.Invoke();
        }

        private void StopInteraction(InputAction.CallbackContext context)
        {
            if (GameManager.CurrentGameState == GameState.Interaction)
            {
                StopInteraction();
            }
        }
    }
}