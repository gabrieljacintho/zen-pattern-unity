using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace FireRingStudio.Assets._Base.Scripts.Random
{
    public class ChildrenOrderRandomizer : MonoBehaviour
    {
        [SerializeField] private bool _randomizeOnEnable;


        private void OnEnable()
        {
            if (_randomizeOnEnable)
            {
                Randomize();
            }
        }

        [Button]
        public void Randomize()
        {
            Transform[] children = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            children.Shuffle();

            foreach (Transform child in children)
            {
                child.SetParent(null);
            }

            foreach (Transform child in children)
            {
                child.SetParent(transform);
            }
        }
    }
}