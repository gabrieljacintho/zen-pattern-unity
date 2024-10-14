using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public class ChildrenModifier : MonoBehaviour
    {
        [Header("Position")]
        public bool overridePosition;
        [ShowIf("overridePosition")]
        public Vector3 position;
        [ShowIf("overridePosition")]
        public Space positionSpace = Space.Self;

        [Header("Rotation")]
        public bool overrideRotation;
        [ShowIf("overrideRotation")]
        public Vector3 rotation;
        [ShowIf("overrideRotation")]
        public Space rotationSpace = Space.Self;
        
        [Header("Scale")]
        public bool overrideScale;
        [ShowIf("overrideScale")]
        public Vector3 scale = Vector3.one;

        [Header("Layer")]
        public bool overrideLayer;
        [ShowIf("overrideLayer")]
        [Range(0, 31)] public int layer;
        [ShowIf("overrideLayer")]
        public bool recursiveLayer;

        [Space]
        public bool applyOnEnable;
        public bool applyEveryFrame;

        private void OnEnable()
        {
            if (applyOnEnable)
                Apply();
        }

        private void Update()
        {
            if (applyEveryFrame)
                Apply();
        }

        [Button]
        public void Apply()
        {
            foreach (Transform child in transform)
            {
                if (overridePosition)
                    ApplyPosition(child);
                
                if (overrideRotation)
                    ApplyRotation(child);
                
                if (overrideScale)
                    ApplyScale(child);

                if (overrideLayer)
                    ApplyLayer(child);
            }
        }

        private void ApplyPosition(Transform child)
        {
            if (positionSpace == Space.Self)
                child.localPosition = position;
            else
                child.position = position;
        }
        
        private void ApplyRotation(Transform child)
        {
            if (rotationSpace == Space.Self)
                child.localRotation = Quaternion.Euler(rotation);
            else
                child.rotation = Quaternion.Euler(rotation);
        }

        private void ApplyScale(Transform child)
        {
            child.localScale = scale;
        }

        private void ApplyLayer(Transform child)
        {
            if (recursiveLayer)
                child.gameObject.SetLayerRecursively(layer);
            else
                child.gameObject.layer = layer;
        }
    }
}