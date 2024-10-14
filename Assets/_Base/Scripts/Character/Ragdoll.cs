using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.Character
{
    public class Ragdoll : MonoBehaviour
    {
        public bool RagdollEnabled { get; private set; }

        public Animator Animator { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Rigidbody[] Rigidbodies { get; private set; }

        public bool enableRagdollOnEnable;


        private void Awake()
        {
            Animator = GetComponent<Animator>();
            if (Animator == null)
                Debug.LogNo(nameof(UnityEngine.Animator));

            Agent = GetComponent<NavMeshAgent>();
            if (Agent == null)
                Debug.LogNo(nameof(NavMeshAgent));
            
            Rigidbodies = GetComponentsInChildren<Rigidbody>();
            if (Rigidbodies == null || Rigidbodies.Length == 0)
                Debug.LogNoInChildren(nameof(Rigidbody));
        }

        private void OnEnable()
        {
            if (enableRagdollOnEnable)
                Enable();
        }

        [HideIf("Enabled")]
        [Button]
        public void Enable()
        {
            if (Animator != null)
                Animator.enabled = false;
            
            if (Agent != null)
                Agent.enabled = false;
            
            if (Rigidbodies != null && Rigidbodies.Length > 0)
            {
                foreach (Rigidbody rigidbody in Rigidbodies)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.useGravity = true;
                    rigidbody.isKinematic = false;
                }
            }

            RagdollEnabled = true;
        }

        [ShowIf("Enabled")]
        [Button]
        public void Disable()
        {
            if (Animator != null)
                Animator.enabled = true;
            
            if (Agent != null)
                Agent.enabled = true;
            
            if (Rigidbodies != null && Rigidbodies.Length > 0)
            {
                foreach (Rigidbody body in Rigidbodies)
                {
                    body.useGravity = false;
                    body.isKinematic = true;
                }
            }

            RagdollEnabled = false;
        }
    }
}
