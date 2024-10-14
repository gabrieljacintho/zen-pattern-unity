using System;
using FireRingStudio.Patterns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;

namespace FireRingStudio.Input
{
    public enum ControlScheme
    {
        KeyboardMouse,
        XboxController,
        PlayStationController
    }

    public class InputManager : PersistentSingleton<InputManager>
    {
        public static ControlScheme CurrentControlScheme { get; private set; }
        
        public delegate void ControlSchemeChangedEvent(ControlScheme controlScheme);
        public static event ControlSchemeChangedEvent ControlSchemeChanged;

        [Space]
        public UnityEvent<ControlScheme> OnControlSchemeChanged;
        
        
        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                return;
            }
            
            ControlSchemeChanged += OnControlSchemeChanged.Invoke;
            InputUser.onChange += OnInputUserChange;
            InputSystem.onDeviceChange += OnDeviceChange;
            
            UpdateControlScheme();
        }

        public static string GetControlSchemeName(ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                case ControlScheme.KeyboardMouse:
                    return "Keyboard Mouse";
                
                case ControlScheme.XboxController:
                    return "Xbox Controller";
                
                case ControlScheme.PlayStationController:
                    return "PlayStation Controller";
                
                default:
                    return string.Empty;
            }
        }

        public static string GetCurrentControlSchemeName()
        {
            return GetControlSchemeName(CurrentControlScheme);
        }
        
        private static void UpdateControlScheme()
        {
            PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
            if (playerInput == null)
            {
                return;
            }
            
            string controlScheme = playerInput.currentControlScheme;
            switch (controlScheme)
            {
                case "Keyboard Mouse":
                    CurrentControlScheme = ControlScheme.KeyboardMouse;
                    break;
                
                case "Gamepad":
                    Gamepad currentGamepad = Gamepad.current;
                    controlScheme = currentGamepad.name;
                    CurrentControlScheme = currentGamepad is DualShockGamepad ? ControlScheme.PlayStationController : ControlScheme.XboxController;
                    break;
                
                case "Xbox Controller":
                    CurrentControlScheme = ControlScheme.XboxController;
                    break;
                
                case "PlayStation Controller":
                    CurrentControlScheme = ControlScheme.PlayStationController;
                    break;
            }

            ControlSchemeChanged?.Invoke(CurrentControlScheme);

            Debug.Log("Control scheme changed: \"" + controlScheme + "\"");
        }

        private static void OnInputUserChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                UpdateControlScheme();
            }
        }

        private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Disconnected && GameManager.InAnyGameState)
            {
                GameManager.SetPaused(true);
            }
        }
    }
}