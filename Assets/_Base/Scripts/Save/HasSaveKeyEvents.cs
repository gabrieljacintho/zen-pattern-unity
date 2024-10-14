using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Save
{
    public class HasSaveKeyEvents : MonoBehaviour
    {
        [SerializeField] private string _saveKey;
        
        [Header("Settings")]
        [SerializeField] private bool _checkOnAwake;
        [SerializeField] private bool _checkOnEnable;

        [Space]
        public UnityEvent onHas;
        public UnityEvent onNotHas;
        

        private void Awake()
        {
            if (_checkOnAwake)
            {
                Check();
            }
        }
        
        private void OnEnable()
        {
            if (_checkOnEnable)
            {
                Check();
            }
        }

        [Button]
        public void Check()
        {
            bool hasKey = PlayerPrefs.HasKey(_saveKey) || ES3.KeyExists(_saveKey);

            if (hasKey)
            {
                onHas?.Invoke();
            }
            else
            {
                onNotHas?.Invoke();
            }
        }
    }
}