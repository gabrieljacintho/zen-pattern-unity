using FireRingStudio.Input;
using FireRingStudio.Patterns;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    public class CursorHandler : NonSingleton<CursorHandler>
    {
        [FormerlySerializedAs("visible")]
        [SerializeField] private bool _visible = true;
        [FormerlySerializedAs("lockMode")]
        [SerializeField] private CursorLockMode _lockMode = CursorLockMode.Confined;
        
        
        protected override void OnEnable()
        {
            base.OnEnable();

            InstanceChanged += UpdateCursor;
            InputManager.ControlSchemeChanged += UpdateCursor;
            
            UpdateCursor();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            UpdateCursor();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            InstanceChanged -= UpdateCursor;
            InputManager.ControlSchemeChanged -= UpdateCursor;
        }

        private void UpdateCursor()
        {
            if (CurrentInstance != this)
                return;
            
            UpdateVisible();
            UpdateLockState();
        }

        private void UpdateCursor(CursorHandler cursorHandler) => UpdateCursor();
        
        private void UpdateCursor(ControlScheme controlScheme) => UpdateCursor();

        private void UpdateVisible()
        {
            bool newVisible = true;
            if (Application.isFocused)
            {
                newVisible = _visible && InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse;
            }

            Cursor.visible = newVisible;
        }

        private void UpdateLockState()
        {
            CursorLockMode lockState = CursorLockMode.None;
            if (Application.isFocused)
            {
                lockState = _lockMode;
            }

            Cursor.lockState = lockState;
        }
    }
}