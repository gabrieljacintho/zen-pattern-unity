using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace FireRingStudio
{
    public class SetCastShadows : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameObjects;
        [SerializeField] private bool _areInstances;


        [Button]
        public void Set(ShadowCastingMode value)
        {
            if (_gameObjects == null)
            {
                return;
            }

            foreach (GameObject gameObject in _gameObjects)
            {
                Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer renderer in renderers)
                {
                    renderer.shadowCastingMode = value;
                }
#if UNITY_EDITOR
                if (!_areInstances)
                {
                    EditorUtility.SetDirty(gameObject);
                }
#endif
            }
#if UNITY_EDITOR
            if (!_areInstances)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("Cast shadows changed.", this);
            }
#endif
        }
    }
}