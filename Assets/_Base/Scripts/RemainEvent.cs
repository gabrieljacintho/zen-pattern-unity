using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class RemainEvent : MonoBehaviour
    {
        [SerializeField] private string _saveKey = "remainevent";
        [SerializeField] private bool _checkOnEnable = true;

        [Space]
        public UnityEvent Event; 


        private void OnEnable()
        {
            if (_checkOnEnable)
            {
                Check();
            }
        }

        public void Invoke()
        {
            Event?.Invoke();

            PlayerPrefs.SetInt(_saveKey, 1);
            PlayerPrefs.Save();
        }

        public void Check()
        {
            if (PlayerPrefs.GetInt(_saveKey) != 1)
            {
                return;
            }

            Event?.Invoke();
        }

        public void ResetEvent()
        {
            PlayerPrefs.DeleteKey(_saveKey);
        }
    }
}