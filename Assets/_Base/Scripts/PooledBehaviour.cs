using FireRingStudio.Extensions;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(PooledObject))]
    public abstract class PooledBehaviour : MonoBehaviour
    {
        protected PooledObject PooledObject;


        protected virtual void Awake()
        {
            PooledObject = gameObject.GetOrAddComponent<PooledObject>();
            PooledObject.OnGetAction += OnGet;
            PooledObject.OnReleaseAction += OnRelease;
        }

        protected virtual void OnGet() { }

        protected virtual void OnRelease() { }
    }
}