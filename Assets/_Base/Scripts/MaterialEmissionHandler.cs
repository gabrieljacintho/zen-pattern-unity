using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialEmissionHandler : MonoBehaviour
    {
        private Material _material;

        private Color _defaultColor;
        
        private static readonly int s_emissionColor = Shader.PropertyToID("_EmissionColor");


        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _defaultColor = _material.GetColor(s_emissionColor);
        }

        [Button]
        public void Enable()
        {
            _material.EnableKeyword("_EMISSION");
            _material.SetColor(s_emissionColor, _defaultColor);
        }

        [Button]
        public void Disable()
        {
            _material.DisableKeyword("_EMISSION");
            _material.SetColor(s_emissionColor, Color.black);
        }
    }
}