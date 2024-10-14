using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.UI
{
    public class SelectedObjectChangedEvent : MonoBehaviour
    {
        public UnityEvent<GameObject> onSelectedObjectChanged;
        
        
        private void OnEnable()
        {
            EventManager.MainSelectedObjectChanged += onSelectedObjectChanged.Invoke;
        }

        private void OnDisable()
        {
            EventManager.MainSelectedObjectChanged -= onSelectedObjectChanged.Invoke;
        }
    }
}