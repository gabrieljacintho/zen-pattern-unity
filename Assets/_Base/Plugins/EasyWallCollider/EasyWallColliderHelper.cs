using Sirenix.OdinInspector;
using UnityEngine;

namespace PepijnWillekens.EasyWallColliderUnity
{
    [ExecuteInEditMode]
    public class EasyWallColliderHelper : MonoBehaviour
    {
        [SerializeField] private Material _defaultMaterial;

#if UNITY_EDITOR
        [Button]
        public void MakeAllRenderers(bool value)
        {
            EasyWallCollider[] wallColliders = FindObjectsOfType<EasyWallCollider>(true);
            foreach (EasyWallCollider wallCollider in wallColliders)
            {
                wallCollider.DefaultMaterial = _defaultMaterial;
                wallCollider.makeRenderers = value;
            }
        }
#endif
    }
}
