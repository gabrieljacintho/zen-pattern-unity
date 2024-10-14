using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Patterns;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace FireRingStudio.Input
{
    public class BindingsManager : PersistentSingleton<BindingsManager>
    {
        [SerializeField] private BindingIcons _keyboardMouseIcons;
        [SerializeField] private BindingIcons _xboxControllerIcons;
        [SerializeField] private BindingIcons _playstationControllerIcons;
        [SerializeField] private PlayerInput _playerInput;
        
        private const string BindingsSaveKey = "Bindings";

        public PlayerInput PlayerInput => _playerInput;

        public delegate void BindingsEvent();
        public static BindingsEvent BindingsUpdated;


        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                return;
            }

            LoadBindings();
        }

        public Sprite GetIcon(InputAction input, ControlScheme controlScheme)
        {
            if (input.FindFirstBindingWithControlScheme(controlScheme, out InputBinding binding))
            {
                return GetIcon(binding.effectivePath, controlScheme);
            }
            
            return GetDefaultIcon(controlScheme);
        }

        public Sprite GetCurrentIcon(InputAction input)
        {
            return GetIcon(input, InputManager.CurrentControlScheme);
        }

        public Sprite GetIcon(InputAction input, int bindingIndex, ControlScheme controlScheme)
        {
            ReadOnlyArray<InputBinding> bindings = input.bindings;

            if (bindings.Count <= bindingIndex)
            {
                return GetDefaultIcon(controlScheme);
            }

            string bindingPath = bindings[bindingIndex].effectivePath;
            return GetIcon(bindingPath, controlScheme);
        }

        public Sprite GetCurrentIcon(InputAction input, int bindingIndex)
        {
            return GetIcon(input, bindingIndex, InputManager.CurrentControlScheme);
        }

        public Sprite GetIcon(string bindingPath, ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                case ControlScheme.KeyboardMouse:
                    return _keyboardMouseIcons != null ? _keyboardMouseIcons.GetIcon(bindingPath) : null;

                case ControlScheme.XboxController:
                    return _xboxControllerIcons != null ? _xboxControllerIcons.GetIcon(bindingPath) : null;

                case ControlScheme.PlayStationController:
                    return _playstationControllerIcons != null ? _playstationControllerIcons.GetIcon(bindingPath) : null;

                default:
                    return null;
            }
        }

        public List<string> GetAllBindingPathsWithIcon()
        {
            List<string> bindingPaths = new List<string>();
            if (_keyboardMouseIcons != null)
            {
                _keyboardMouseIcons.BindingIconsList?.ForEach(bindingIcon => bindingPaths.Add(bindingIcon.BindingPath));
            }
            
            if (_xboxControllerIcons != null)
            {
                _xboxControllerIcons.BindingIconsList?.ForEach(bindingIcon => bindingPaths.Add(bindingIcon.BindingPath));
            }
            
            if (_playstationControllerIcons != null)
            {
                _playstationControllerIcons.BindingIconsList?.ForEach(bindingIcon => bindingPaths.Add(bindingIcon.BindingPath));
            }
            
            List<string> temp = new List<string>(bindingPaths);
            foreach (string bindingPath in temp)
            {
                if (bindingPath.Contains("<Gamepad>"))
                {
                    string newPath = bindingPath.Replace("<Gamepad>", "<XInputController>");
                    bindingPaths.Add(newPath);
                    
                    newPath = bindingPath.Replace("<Gamepad>", "<DualShockGamepad>");
                    bindingPaths.Add(newPath);
                    
                    newPath = bindingPath.Replace("<Gamepad>", "<DualSenseGamepadHID>");
                    bindingPaths.Add(newPath);
                }
            }

            return bindingPaths;
        }

        #region Load/Save

        public void LoadBindings()
        {
            if (_playerInput == null)
            {
                return;
            }

            string rebinds = PlayerPrefs.GetString(BindingsSaveKey, string.Empty);
            if (string.IsNullOrEmpty(rebinds))
            {
                return;
            }

            _playerInput.actions.LoadBindingOverridesFromJson(rebinds);

            BindingsUpdated?.Invoke();
        }

        public void SaveBindings()
        {
            if (_playerInput == null)
            {
                return;
            }

            string rebinds = _playerInput.actions.SaveBindingOverridesAsJson();

            PlayerPrefs.SetString(BindingsSaveKey, rebinds);
            PlayerPrefs.Save();
        }

        public void ResetBindings()
        {
            if (_playerInput == null)
            {
                return;
            }

            _playerInput.actions.RemoveAllBindingOverrides();

            PlayerPrefs.DeleteKey(BindingsSaveKey);
            PlayerPrefs.Save();

            BindingsUpdated?.Invoke();
        }

        #endregion

        private Sprite GetDefaultIcon(ControlScheme controlScheme)
        {
            switch (controlScheme)
            {
                case ControlScheme.KeyboardMouse:
                    return _keyboardMouseIcons != null ? _keyboardMouseIcons.DefaultIcon : null;

                case ControlScheme.XboxController:
                    return _xboxControllerIcons != null ? _xboxControllerIcons.DefaultIcon : null;

                case ControlScheme.PlayStationController:
                    return _playstationControllerIcons != null ? _playstationControllerIcons.DefaultIcon : null;

                default:
                    return null;
            }
        }
    }
}