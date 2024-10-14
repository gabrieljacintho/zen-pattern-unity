using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    [RequireComponent(typeof(MeshRenderer))]
    public class LightMaterialEmissionHandler : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [FormerlySerializedAs("defaultLightIntensity")]
        [SerializeField] private float _totalLightIntensity = 1f;
        
        private MeshRenderer _meshRenderer;
        private readonly Dictionary<Material, Color> _emissionColors = new Dictionary<Material, Color>();

        private float _lastLightIntensity = -1f;


        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            LoadEmissionColors();
        }

        private void Update()
        {
            if (_meshRenderer.materials == null)
            {
                return;
            }

            float lightIntensity = _light != null && _light.enabled ? _light.intensity : 0f;
            if (lightIntensity == _lastLightIntensity)
            {
                return;
            }

            foreach (Material material in _meshRenderer.materials)
            {
                UpdateMaterial(material, lightIntensity);
            }

            _lastLightIntensity = lightIntensity;
        }

        private void UpdateMaterial(Material material, float lightIntensity)
        {
            float emissionIntensity = lightIntensity / _totalLightIntensity;

            Color emissionColor = Color.white;
            if (_emissionColors.TryGetValue(material, out Color color))
            {
                emissionColor = color;
            }
                
            material.SetEmissionColor(emissionColor * emissionIntensity);
        }

        private void LoadEmissionColors()
        {
            if (_meshRenderer.materials == null || _meshRenderer.materials.Length > 0)
            {
                return;
            }
            
            foreach (Material material in _meshRenderer.materials)
            {
                _emissionColors.Add(material, material.GetEmissionColor());
            }
        }
    }
}