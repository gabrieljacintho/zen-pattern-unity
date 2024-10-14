using System;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio
{
    [Serializable]
    public class RendererSettings
    {
        [SerializeField] private Renderer _renderer;
        [Tooltip("If null the renderer material is used.")]
        [SerializeField] private MaterialSettings[] _fadeMaterialSettingsArray = new MaterialSettings[1];
        
        private Material[] _defaultMaterials;
        
        private bool _initialized;

        public Renderer Renderer => _renderer;
        public MaterialSettings[] FadeMaterialSettingsArray => _fadeMaterialSettingsArray;


        public RendererSettings(Renderer renderer, MaterialSettings[] fadeMaterialSettingsArray)
        {
            _renderer = renderer;
            _fadeMaterialSettingsArray = fadeMaterialSettingsArray;
        }
        
        public void SetAlpha(float value)
        {
            if (!_initialized)
            {
                Initialize();
            }
            
            value = Mathf.Clamp(value, 0f, 1f);
            
            if (_fadeMaterialSettingsArray == null)
            {
                return;
            }
            
            foreach (MaterialSettings fadeMaterialSettings in _fadeMaterialSettingsArray)
            {
                fadeMaterialSettings.Alpha = value;
            }
        }
        
        public void SetMaterialsToFade()
        {
            if (!_initialized)
            {
                Initialize();
            }
            
            if (_renderer == null)
            {
                return;
            }

            _renderer.materials = GetFadeMaterials();
            
            SetFadeMaterialInstances(_renderer.materials);
        }
        
        public void SetMaterialsToDefault()
        {
            if (!_initialized)
            {
                Initialize();
            }
            
            if (_renderer != null)
            {
                _renderer.materials = _defaultMaterials;
            }
        }
        
        public void DestroyMaterials()
        {
            if (_renderer != null)
            {
                _renderer.DestroyMaterials();
            }
            
            if (_fadeMaterialSettingsArray != null)
            {
                foreach (MaterialSettings materialSettings in _fadeMaterialSettingsArray)
                {
                    materialSettings.DestroyMaterialInstance();
                }
            }
        }

        private Material[] GetFadeMaterials()
        {
            if (_fadeMaterialSettingsArray == null)
            {
                return null;
            }

            Material[] materials = new Material[_fadeMaterialSettingsArray.Length];
            for (int i = 0; i < _fadeMaterialSettingsArray.Length; i++)
            {
                MaterialSettings materialSettings = _fadeMaterialSettingsArray[i];
                Material material = null;

                if (materialSettings.MaterialInstance != null)
                {
                    material = materialSettings.MaterialInstance;
                }
                else if (materialSettings.Material != null)
                {
                    material = materialSettings.Material;
                }
                else if (_renderer != null)
                {
                    Material[] rendererMaterials = _renderer.materials;
                    if (rendererMaterials != null && rendererMaterials.Length > i)
                    {
                        material = new Material(rendererMaterials[i]);
                        materialSettings.MaterialInstance = material;
                    }
                }
                
                materials[i] = material;
                
            }

            return materials;
        }
        
        private void SetFadeMaterialInstances(Material[] values)
        {
            if (_fadeMaterialSettingsArray == null)
            {
                return;
            }
            
            for (int i = 0; i < _fadeMaterialSettingsArray.Length; i++)
            {
                MaterialSettings materialSettings = _fadeMaterialSettingsArray[i];
                materialSettings.MaterialInstance = values.Length > i ? values[i] : null;
            }
        }

        private void Initialize()
        {
            if (_renderer == null)
            {
                return;
            }
            
            _defaultMaterials = _renderer.materials;

            _initialized = true;
        }
    }
}