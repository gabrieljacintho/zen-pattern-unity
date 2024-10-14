using System;
using FireRingStudio.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FireRingStudio
{
    [Serializable]
    public class MaterialSettings
    {
        [SerializeField] private Material _material;
        [SerializeField] private string _colorPropertyName = "_Color";

        private Material _materialInstance;
        
        private float _defaultAlpha;
        
        private bool _initialized;

        public Material Material
        {
            get => _material;
            set => SetMaterial(value);
        }
        public Material MaterialInstance
        {
            get => _materialInstance;
            set => _materialInstance = value;
        }
        public float Alpha
        {
            get => _material != null ? _material.GetAlpha(_colorPropertyName) : 0f;
            set => SetAlpha(value);
        }
        
        
        public void SetMaterial(Material value)
        {
            if (_materialInstance == value)
            {
                return;
            }
            
            _material = value;
            
            _initialized = false;
        }
        
        public void SetAlpha(float value)
        {
            if (!_initialized)
            {
                Initialize();
            }
            
            if (_materialInstance == null)
            {
                return;
            }
            
            _materialInstance.SetAlpha(_colorPropertyName, value);
        }
        
        public void ResetAlpha()
        {
            if (!_initialized)
            {
                Initialize();
            }

            Alpha = _defaultAlpha;
        }
        
        public void DestroyMaterialInstance()
        {
            if (_materialInstance != null)
            {
                Object.Destroy(_materialInstance);
            }
        }
        
        private void Initialize()
        {
            if (_material == null)
            {
                return;
            }

            _defaultAlpha = _material.GetAlpha(_colorPropertyName);
            
            _initialized = true;
        }
    }
}