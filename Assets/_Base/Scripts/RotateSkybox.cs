using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public class RotateSkybox : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.4f;
        [SerializeField] private bool _setRotationOnEnable;
        [ShowIf("_setRotationOnEnable")]
        [SerializeField] private float _rotationOnEnable;

        private Material _skyboxMaterial;
        
        private static readonly int s_rotation = Shader.PropertyToID("_Rotation");


        private void Awake()
        {
            _skyboxMaterial = RenderSettings.skybox;
        }

        private void OnEnable()
        {
            if (_setRotationOnEnable)
            {
                SetRotation(_rotationOnEnable);
            }
        }

        private void Update()
        {
            if (_speed == 0f || GameManager.IsPaused)
            {
                return;
            }

            float rotation = _skyboxMaterial.GetFloat(s_rotation);
            rotation += _speed * Time.unscaledDeltaTime;

            SetRotation(rotation);
        }

        [Button]
        public void SetRotation(float value)
        {
            if (_skyboxMaterial == null)
            {
                return;
            }

            _skyboxMaterial.SetFloat(s_rotation, value);
        }
    }
}
