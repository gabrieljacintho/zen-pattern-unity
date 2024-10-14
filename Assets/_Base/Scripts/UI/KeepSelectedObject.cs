using UnityEngine;
using UnityEngine.EventSystems;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(EventSystem))]
    public class KeepSelectedObject : MonoBehaviour
    {
        public GameObject LastSelectedObject { get; private set; }
        
        public EventSystem EventSystem { get; private set; }


        private void Awake()
        {
            EventSystem = GetComponent<EventSystem>();
        }
 
        private void Update()
        {
            if (EventSystem == null)
            {
                return;
            }

            if (EventSystem.currentSelectedGameObject != null)
            {
                LastSelectedObject = EventSystem.currentSelectedGameObject;
            }
            else
            {
                EventSystem.SetSelectedGameObject(LastSelectedObject);
            }
        }
    }
}