using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterPush : MonoBehaviour
    {
        [Range(0f, 5f)] public float strength = 1f;
        public LayerMask layerMask = ~0;

        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
            if (attachedRigidbody == null || attachedRigidbody.isKinematic)
                return;

            if (!layerMask.Contains(attachedRigidbody.gameObject.layer))
                return;

            if (hit.moveDirection.y < -0.3f)
                return;

            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
            attachedRigidbody.AddForce(pushDirection * strength, ForceMode.Impulse);
        }
    }
}
