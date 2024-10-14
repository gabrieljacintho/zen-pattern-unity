using System;
using System.Collections.Generic;
using FireRingStudio.UI;
using UnityEngine.UI;

namespace FireRingStudio.Extensions
{
    public static class UIElementsExtensions
    {
        public static void SetOnClickListener(this List<Button> buttons, Action<Button> action, bool add)
        {
            foreach (Button button in buttons)
            {
                button.SetOnClickListener(action, add);
            }
        }
        
        public static void SetOnClickListener(this Button button, Action<Button> action, bool add)
        {
            void OnClick()
            {
                action?.Invoke(button);
            }

            if (add)
            {
                button.onClick.AddListener(OnClick);
            }
            else
            {
                button.onClick.RemoveListener(OnClick);
            }
        }
        
        public static void SetOnClickListener<T>(this List<T> iButtons, Action<T> action, bool add) where T : IButton
        {
            foreach (T iButton in iButtons)
            {
                iButton.SetOnClickListener(action, add);
            }
        }
        
        public static bool SetOnClickListener<T>(this T iButton, Action<T> action, bool add) where T : IButton
        {
            Button button = iButton.Button;
            if (button == null)
            {
                return false;
            }
            
            void OnClick()
            {
                action?.Invoke(iButton);
            }

            if (add)
            {
                button.onClick.AddListener(OnClick);
            }
            else
            {
                button.onClick.RemoveListener(OnClick);
            }

            return true;
        }
    }
}