using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace FireRingStudio
{
    public class BakeSkinnedMesh : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [Tooltip("A static mesh that will receive the snapshot of the skinned mesh.")]
        [SerializeField] private Mesh _mesh;
        [ShowIf("@_mesh == null"), FolderPath(ParentFolder = "Assets")]
        [SerializeField] private string _meshFolderPath;
        [ShowIf("@_mesh == null")]
        [SerializeField] private string _meshFileName;
        [Tooltip("Whether to use the SkinnedMeshRenderer's Transform scale when baking the Mesh. If this is true, Unity bakes the Mesh using the position, rotation, and scale values from the SkinnedMeshRenderer's Transform. If this is false, Unity bakes the Mesh using the position and rotation values from the SkinnedMeshRenderer's Transform, but without using the scale value from the SkinnedMeshRenderer's Transform. The default value is false.")]
        [SerializeField] private bool _useScale;
        [SerializeField] private bool _replaceRenderer;


        [Button]
        public void Bake()
        {
#if UNITY_EDITOR
            SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderer;

            if (skinnedMeshRenderer == null)
            {
                skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            }

            if (_mesh == null)
            {
                _mesh = new Mesh();
                string path = "Assets/" + _meshFolderPath + "/" + _meshFileName + ".mesh";
                AssetDatabase.CreateAsset(_mesh, path);
            }

            skinnedMeshRenderer.BakeMesh(_mesh, _useScale);

            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(_mesh);

            if (_replaceRenderer)
            {
                ReplaceRenderer(skinnedMeshRenderer, _mesh);
            }
#endif
        }

        private void ReplaceRenderer(SkinnedMeshRenderer skinnedMeshRenderer, Mesh mesh)
        {
            MeshFilter meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
            meshRenderer.materials = skinnedMeshRenderer.sharedMaterials;
            meshRenderer.shadowCastingMode = skinnedMeshRenderer.shadowCastingMode;
            meshRenderer.lightProbeUsage = skinnedMeshRenderer.lightProbeUsage;

            DestroyImmediate(skinnedMeshRenderer);
        }
    }
}