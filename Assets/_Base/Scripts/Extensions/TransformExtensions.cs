using System.Collections.Generic;
using System.Linq;
using FireRingStudio.Helpers;
using TMPro;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class TransformExtensions
    {
        public static void RandomPosition(this Transform transform, Vector3 range, Space space = Space.Self)
        {
            Vector3 randomValue = VectorHelper.Random(range);
            if (space == Space.Self)
            {
                transform.localPosition = randomValue;
            }
            else
            {
                transform.position = randomValue;
            }
        }

        public static void RandomRotation(this Transform transform, Vector3 range, Space space = Space.Self)
        {
            Quaternion randomValue = QuaternionHelper.Random(range);
            if (space == Space.Self)
            {
                transform.localRotation = randomValue;
            }
            else
            {
                transform.rotation = randomValue;
            }
        }

        public static void RandomScale(this Transform transform, Vector3 range)
        {
            transform.localScale = VectorHelper.Random(range);
        }

        public static TransformValues Values(this Transform transform)
        {
            return new TransformValues(transform);
        }
        
        public static Dictionary<Transform, TransformValues> AllValues(this Transform transform)
        {
            Dictionary<Transform, TransformValues> transformValues = new();
            
            transformValues.Add(transform, transform.Values());
            
            foreach (Transform child in transform)
            {
                Dictionary<Transform, TransformValues> childValues = AllValues(child);
                transformValues.AddRange(childValues);
            }

            return transformValues;
        }

        public static void SetValues(this Transform transform, TransformValues values, Space space = Space.Self)
        {
            if (space == Space.Self)
            {
                transform.localPosition = values.Position;
                transform.localRotation = values.Rotation;
            }
            else
            {
                transform.position = values.Position;
                transform.rotation = values.Rotation;
            }

            transform.localScale = values.Scale;
        }

        public static void LoadValues(this Dictionary<Transform, TransformValues> values, Space space = Space.Self)
        {
            foreach (KeyValuePair<Transform, TransformValues> transformValue in values)
            {
                if (transformValue.Key != null)
                    transformValue.Key.SetValues(transformValue.Value, space);
            }
        }

        public static Transform[] GetChildren(this Transform transform)
        {
            int childCount = transform.childCount;
            Transform[] children = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children;
        }

        public static List<Transform> GetChildrenRecursively(this Transform transform)
        {
            List<Transform> children = transform.GetChildren().ToList();

            List<Transform> otherChildren = new List<Transform>();
            foreach (Transform child in children)
            {
                otherChildren.AddRange(child.GetChildrenRecursively());
            }
            
            children.AddRange(otherChildren);

            return children;
        }

        public static Transform[] GetSiblings(this Transform transform)
        {
            return transform.parent.GetChildren();
        }

        public static bool ContainsChild(this Transform transform, Transform target, bool checkRecursively = false)
        {
            if (checkRecursively)
            {
                List<Transform> children = transform.GetChildrenRecursively();
                return children.Contains(target);
            }
            else
            {
                Transform[] children = transform.GetChildren();
                return children.Contains(target);
            }
        }

        public static bool ContainsSibling(this Transform transform, Transform target)
        {
            Transform[] siblings = transform.GetSiblings();
            return siblings.Contains(target);
        }

        public static void SetActiveChildren(this Transform transform, bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }
        }

        public static List<Transform> OrderByDistance(this Transform[] transforms, Vector3 originPosition)
        {
            return transforms.OrderBy(transform => Vector3.Distance(originPosition, transform.position)).ToList();
        }

        public static bool CheckAngle(this Transform transform, Vector3 targetPosition, float angle)
        {
            return transform.position.CheckAngle(targetPosition, transform.forward, angle);
        }

        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildrenImmediate(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}