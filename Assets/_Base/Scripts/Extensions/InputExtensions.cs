using FireRingStudio.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace FireRingStudio.Extensions
{
    public static class InputExtensions
    {
        #region Icon

        public static Sprite GetIcon(this InputAction input, ControlScheme controlScheme)
        {
            return BindingsManager.Instance != null ? BindingsManager.Instance.GetIcon(input, controlScheme) : null;
        }

        public static Sprite GetIcon(this InputAction input, int bindingIndex, ControlScheme controlScheme)
        {
            return BindingsManager.Instance != null ? BindingsManager.Instance.GetIcon(input, bindingIndex, controlScheme) : null;
        }

        public static Sprite GetIcon(this InputAction input)
        {
            return input.GetIcon(InputManager.CurrentControlScheme);
        }

        #endregion

        #region DisplayName

        public static string GetDisplayName(this InputAction input, ControlScheme controlScheme,
            InputBinding.DisplayStringOptions option = InputBinding.DisplayStringOptions.DontIncludeInteractions)
        {
            if (FindFirstBindingWithControlScheme(input, controlScheme, out InputBinding binding))
            {
                return binding.ToDisplayString(option);
            }

            return string.Empty;
        }

        public static string GetCurrentDisplayName(this InputAction input, InputBinding.DisplayStringOptions option = InputBinding.DisplayStringOptions.DontIncludeInteractions)
        {
            return GetDisplayName(input, InputManager.CurrentControlScheme);
        }

        public static string GetDisplayName(this InputAction input, int bindingIndex, ControlScheme controlScheme,
            InputBinding.DisplayStringOptions option = InputBinding.DisplayStringOptions.DontIncludeInteractions)
        {
            ReadOnlyArray<InputBinding> bindings = input.bindings;

            if (bindings.Count <= bindingIndex)
            {
                return string.Empty;
            }

            return bindings[bindingIndex].ToDisplayString(option);
        }

        public static string GetCurrentDisplayName(this InputAction input, int bindingIndex,
            InputBinding.DisplayStringOptions option = InputBinding.DisplayStringOptions.DontIncludeInteractions)
        {
            return GetDisplayName(input, bindingIndex, InputManager.CurrentControlScheme, option);
        }

        #endregion

        #region NameLocalized

        public static string GetNameLocalized(this InputAction input)
        {
            return input.name.GetLocalized("Controls");
        }
        
        public static string GetNameLocalized(this InputActionReference inputReference)
        {
            return inputReference.action.GetNameLocalized();
        }
        
        public static string GetInteractionLocalized(this InputActionReference inputReference, string defaultInteraction = "Press")
        {
            string interaction = inputReference.action.bindings[0].interactions.GetUntil(";");

            switch (interaction)
            {
                case "Press":
                    return "Press".GetLocalized("Controls");

                case "Tap":
                    return "Tap".GetLocalized("Controls");

                case "Hold":
                    return "Hold".GetLocalized("Controls");

                default:
                    if (!string.IsNullOrEmpty(defaultInteraction))
                    {
                        return defaultInteraction.GetLocalized("Controls");
                    }

                    return string.Empty;
            }
        }

        #endregion

        public static bool FindFirstBindingWithControlScheme(this InputAction input, ControlScheme controlScheme, out InputBinding binding)
        {
            ReadOnlyArray<InputBinding> bindings = input.bindings;

            foreach (InputBinding otherBinding in bindings)
            {
                string groups = otherBinding.groups;

                switch (controlScheme)
                {
                    case ControlScheme.KeyboardMouse:
                        if (groups.Contains("Keyboard Mouse"))
                        {
                            binding = otherBinding;
                            return true;
                        }
                        break;

                    case ControlScheme.XboxController:
                        if (groups.Contains("Xbox Controller"))
                        {
                            binding = otherBinding;
                            return true;
                        }
                        break;

                    case ControlScheme.PlayStationController:
                        if (groups.Contains("PlayStation Controller"))
                        {
                            binding = otherBinding;
                            return true;
                        }
                        break;
                }
            }

            foreach (InputBinding otherBinding in bindings)
            {
                string groups = otherBinding.groups;

                if (string.IsNullOrEmpty(groups))
                {
                    binding = otherBinding;
                    return true;
                }
            }

            binding = default;
            return false;
        }

        public static bool HasBindingWithControlScheme(this InputAction input, ControlScheme controlScheme)
        {
            ReadOnlyArray<InputBinding> bindings = input.bindings;
            foreach (InputBinding binding in bindings)
            {
                string groups = binding.groups;
                switch (controlScheme)
                {
                    case ControlScheme.KeyboardMouse:
                        if (groups.Contains("Keyboard Mouse"))
                        {
                            return true;
                        }
                        break;
                    
                    case ControlScheme.XboxController:
                        if (groups.Contains("Xbox Controller"))
                        {
                            return true;
                        }
                        break;
                    
                    case ControlScheme.PlayStationController:
                        if (groups.Contains("PlayStation Controller"))
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }
    }
}