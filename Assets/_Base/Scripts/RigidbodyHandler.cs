using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public class RigidbodyHandler : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;

        public Rigidbody[] Rigidbodies
        {
            get
            {
                if (_rigidbodies == null)
                {
                    _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
                }

                return _rigidbodies;
            }
        }
        
        
        [Button]
        public void ResetVelocity()
        {
            foreach (Rigidbody rigidbody in Rigidbodies)
            {
                if (rigidbody != null)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}