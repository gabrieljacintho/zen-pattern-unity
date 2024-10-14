using UnityEngine;

namespace FireRingStudio.Vitals
{
    public class DamageIndicator : MonoBehaviour
    {
        public Transform PlayerTransform { get; private set; }
        public Damage Damage { get; private set; }


        private void LateUpdate()
        {
            if (PlayerTransform == null)
                return;

            Vector3 direction = (Damage.Position - PlayerTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float angle = -targetRotation.eulerAngles.y + PlayerTransform.eulerAngles.y;
            targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.localRotation = targetRotation;
        }
        
        public void Initialize(Transform playerTransform, Damage damage)
        {
            PlayerTransform = playerTransform;
            Damage = damage;
        }
    }
}
