using System;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class CurrentYearLoader : MonoBehaviour
    {
        [SerializeField] private IntVariable _targetVariable;

        [Space]
        public UnityEvent onLoad;


        private void Start()
        {
            Load();
        }

        public void Load()
        {
            if (_targetVariable == null)
                return;

            _targetVariable.Value = DateTime.Now.Year;
            
            onLoad?.Invoke();
        }
    }
}