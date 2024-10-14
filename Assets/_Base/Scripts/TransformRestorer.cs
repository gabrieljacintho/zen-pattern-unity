using System.Collections.Generic;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class TransformRestorer : MonoBehaviour
    {
        public bool restoreOnEnable;
        
        [Space]
        public UnityEvent onRestore;
        
        private Dictionary<Transform, TransformValues> _defaultTransformValues;


        private void Awake()
        {
            CacheValues();
        }

        private void OnEnable()
        {
            if (restoreOnEnable)
                Restore();
        }

        [Button]
        public void CacheValues()
        {
            _defaultTransformValues = transform.AllValues();
        }

        [Button]
        public void Restore()
        {
            _defaultTransformValues?.LoadValues();

            onRestore?.Invoke();
        }
    }
}