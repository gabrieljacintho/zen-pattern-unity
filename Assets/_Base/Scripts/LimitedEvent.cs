using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class LimitedEvent : MonoBehaviour
    {
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private int _maxInvokes = 1;
        [SerializeField] private bool _invokeOnEnable;
        [SerializeField] private bool _countOnInvoke;
        [SerializeField] private bool _disableObjectOnInvokeAll;
        [SerializeField] private IntVariable _invokeCountVariable;
        [SerializeField] private bool _useSave = true;
        [ShowIf("_useSave")]
        [SerializeField] private string _saveKey = "IntroEvent";

        private int _invokeCount;

        private int InvokeCount
        {
            get => _invokeCountVariable != null ? _invokeCountVariable.Value : _invokeCount;
            set
            {
                if (_invokeCountVariable != null)
                {
                    _invokeCountVariable.Value = value;
                }

                _invokeCount = value;

                Save();
                OnInvokeCountChanged();
            }
        }
        private bool CanInvoke => _maxInvokes < 0 || InvokeCount < _maxInvokes;

        [Space]
        public UnityEvent Event;
        public UnityEvent OnInvokeAll;


        private void OnEnable()
        {
            Load();

            if (_invokeOnEnable)
            {
                Invoke();
            }
        }

        [Button]
        public void Invoke()
        {
            if (!CanInvoke)
            {
                return;
            }
            
            Event?.Invoke();

            if (_countOnInvoke)
            {
                Count();
            }
        }

        [Button]
        public void Count()
        {
            InvokeCount++;
        }

        public void ResetCount()
        {
            InvokeCount = 0;
        }

        private void OnInvokeCountChanged()
        {
            if (CanInvoke)
            {
                return;
            }
            
            OnInvokeAll?.Invoke();

            if (_disableObjectOnInvokeAll)
            {
                gameObject.SetActive(false);
            }
        }

        private void Load()
        {
            if (!_useSave)
            {
                return;
            }

            InvokeCount = PlayerPrefs.GetInt(_saveKey);
        }

        private void Save()
        {
            if (!_useSave)
            {
                return;
            }

            PlayerPrefs.SetInt(_saveKey, InvokeCount);
            PlayerPrefs.Save();
        }
    }
}