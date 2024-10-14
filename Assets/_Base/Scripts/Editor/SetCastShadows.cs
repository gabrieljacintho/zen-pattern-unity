using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace FireRingStudio
{
    public class SetCastShadows : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _prefabs;


        [Button]
        public void Set(ShadowCastingMode value)
        {
            if (_prefabs == null)
            {
                return;
            }

            foreach (GameObject gameObject in _prefabs)
            {
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer renderer in renderers)
                {
                    renderer.shadowCastingMode = value;
                }

                EditorUtility.SetDirty(gameObject);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Cast shadows changed.", this);
        }
    }
}