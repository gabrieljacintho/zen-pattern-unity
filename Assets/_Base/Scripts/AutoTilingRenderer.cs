using UnityEngine;

namespace FireRingStudio
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class AutoTilingRenderer : MonoBehaviour
    {
        [SerializeField] private float _textureToMeshZ = 1f;

        private Renderer _renderer;
        private Texture _texture;
        
        private Vector3 _previousScale = Vector3.one;
        private float _previousTextureToMeshZ = -1f;

        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _texture = _renderer.material.mainTexture;
            _previousScale = gameObject.transform.lossyScale;
            _previousTextureToMeshZ = _textureToMeshZ;
            UpdateTiling();
        }

        private void Update()
        {
            if (gameObject.transform.lossyScale != _previousScale || !Mathf.Approximately(_textureToMeshZ, _previousTextureToMeshZ))
            {
                UpdateTiling();
            }

            _previousScale = gameObject.transform.lossyScale;
            _previousTextureToMeshZ = _textureToMeshZ;
        }

        [ContextMenu("Update Tiling")]
        public void UpdateTiling()
        {
            // A Unity plane is 10 units x 10 units
            const float PlaneSizeX = 10f;
            const float PlaneSizeZ = 10f;

            // Figure out texture-to-mesh width based on user set texture-to-mesh height
            float textureToMeshX = (float)_texture.width / _texture.height * _textureToMeshZ;

            Vector3 scale = gameObject.transform.lossyScale;
            float x = PlaneSizeX * scale.x / textureToMeshX;
            float y = PlaneSizeZ * scale.z / _textureToMeshZ;
            
            _renderer.material.mainTextureScale = new Vector2(x, y);
        }
    }
}