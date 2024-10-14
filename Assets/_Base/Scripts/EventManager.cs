using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FireRingStudio
{
    public static class EventManager
    {
        private static EventSystem s_eventSystem;
        private static GameObject s_mainSelectedObject;
        
        public static EventSystem EventSystem
        {
            get
            {
                if (s_eventSystem == null)
                {
                    s_eventSystem = EventSystem.current;
                }

                return s_eventSystem;
            }
        }

        public static GameObject SelectedObject
        {
            get => EventSystem != null ? EventSystem.currentSelectedGameObject : null;
            set
            {
                if (EventSystem != null)
                {
                    EventSystem.SetSelectedGameObject(value);
                    MainSelectedObject = value;
                }
            }
        }

        public static GameObject MainSelectedObject
        {
            get => s_mainSelectedObject;
            set
            {
                if (s_mainSelectedObject != value)
                {
                    s_mainSelectedObject = value;
                    MainSelectedObjectChanged?.Invoke(s_mainSelectedObject);
                }
            }
        }

        public static Action<GameObject> MainSelectedObjectChanged;
    }
}