using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Input
{
    [Serializable]
    public struct BindingIcon
    {
        public string BindingPath;
        public Sprite Icon;
    }
    
    [CreateAssetMenu(menuName = "FireRing Studio/Input/Binding Icons", fileName = "New Binding Icons")]
    public class BindingIcons : ScriptableObject
    {
        public List<BindingIcon> BindingIconsList;
        public Sprite DefaultIcon;


        public Sprite GetIcon(string bindingPath)
        {
            if (HasBinding(bindingPath))
            {
                return FindIcon(bindingPath);
            }

            return DefaultIcon;
        }

        private Sprite FindIcon(string bindingPath)
        {
            bindingPath = FixBindingPath(bindingPath);
            return BindingIconsList != null ? BindingIconsList.Find(bindingIcon => bindingIcon.BindingPath == bindingPath).Icon : null;
        }

        public bool HasBinding(string bindingPath)
        {
            bindingPath = FixBindingPath(bindingPath);
            return BindingIconsList != null && BindingIconsList.Exists(bindingIcon => bindingIcon.BindingPath == bindingPath);
        }
        
        private static string FixBindingPath(string path)
        {
            if (path.Contains("<XInputController>"))
            {
                path = path.Replace("<XInputController>", "Gamepad");
            }
            else if (path.Contains("<DualShockGamepad>"))
            {
                path = path.Replace("<DualShockGamepad>", "Gamepad");
            }
            else if (path.Contains("<DualSenseGamepadHID>"))
            {
                path = path.Replace("<DualSenseGamepadHID>", "Gamepad");
            }

            return path;
        }
    }
}