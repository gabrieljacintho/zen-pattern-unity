using ch.sycoforge.Decal;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Assets._AssetBase.Scripts
{
    public class EasyDecalReplacer : MonoBehaviour
    {
        [Button]
        public void Replace()
        {
            transform.DestroyChildrenImmediate();

            EasyDecal[] decals = FindObjectsOfType<EasyDecal>(true);

            foreach (EasyDecal decal in decals)
            {
                CloneDecal(decal);
            }
        }

        [Button]
        public void Add(Transform root)
        {
            EasyDecal[] decals = root.GetComponentsInChildren<EasyDecal>();

            foreach (EasyDecal decal in decals)
            {
                CloneDecal(decal);
            }
        }

        private void CloneDecal(EasyDecal easyDecal)
        {
            GameObject newGameObject = new GameObject(easyDecal.transform.parent.name + "_" + easyDecal.gameObject.name);

            Transform newTransform = newGameObject.transform;
            newTransform.position = easyDecal.transform.position;
            newTransform.rotation = easyDecal.transform.rotation;
            newTransform.localScale = easyDecal.transform.localScale;
            newTransform.parent = transform;

            EasyDecal newDecal = newGameObject.AddComponent<EasyDecal>();
            newDecal.enabled = false;
            newDecal.Mask = easyDecal.Mask;
            newDecal.Technique = easyDecal.Technique;
            newDecal.Distance = easyDecal.Distance;
            newDecal.Mode = easyDecal.Mode;
            newDecal.DecalMaterial = easyDecal.DecalMaterial;
        }
    }
}