using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace FireRingStudio.Input
{
    public abstract class InputComponent : MonoBehaviour
    {
        [FormerlySerializedAs("_input")]
        [FormerlySerializedAs("input")]
        [SerializeField] private InputActionReference _inputReference;
        [SerializeField] private InputAction _input;
        [SerializeField] private bool _onlyInGame;
        [SerializeField] private bool _developmentOnly;
        
        
        protected virtual void Awake()
        {
            _input?.Enable();
        }
        
        protected virtual void OnEnable()
        {
            this.DoOnNextFrame(() =>
            {
                if (_inputReference != null)
                {
                    _inputReference.action.Reset();
                    _inputReference.action.performed += OnPerformFunc;
                }
                
                if (_input != null)
                {
                    _input.Reset();
                    _input.performed += OnPerformFunc;
                }
            });
        }

        protected virtual void OnDisable()
        {
            if (_inputReference != null)
            {
                _inputReference.action.performed -= OnPerformFunc;
            }
            
            if (_input != null)
            {
                _input.performed -= OnPerformFunc;
            }
        }
        
        protected abstract void OnPerformFunc();

        private void OnPerformFunc(InputAction.CallbackContext context)
        {
            if (_onlyInGame && !GameManager.InGame)
            {
                return;
            }
            
            if (_developmentOnly && !SymbolsHelper.IsInDevelopment())
            {
                return;
            }

            OnPerformFunc();
        }
    }
}