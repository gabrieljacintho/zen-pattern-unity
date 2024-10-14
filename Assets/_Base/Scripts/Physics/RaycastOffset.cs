using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Physics
{
    public class RaycastOffset : MonoBehaviour
    {
        [SerializeField] private Vector3 _originOffset;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction;
        [SerializeField] private Vector3 _offset;


        private void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.TransformPoint(_originOffset);
            Vector3 direction = transform.TransformDirection(_direction);

            Gizmos.DrawWireSphere(origin, 0.1f);
            Gizmos.DrawRay(origin, direction);
        }

        public void Setup(Vector3 originOffset, Vector3 direction, LayerMask layerMask, QueryTriggerInteraction triggerInteraction, Vector3 offset)
        {
            _originOffset = originOffset;
            _direction = direction;
            _layerMask = layerMask;
            _triggerInteraction = triggerInteraction;
            _offset = offset;
        }

        [Button]
        public void UpdateOffset()
        {
            Vector3 origin = transform.TransformPoint(_originOffset);
            Vector3 direction = transform.TransformDirection(_direction);

            RaycastHit[] hits = UnityEngine.Physics.RaycastAll(origin, direction, Mathf.Infinity, _layerMask, _triggerInteraction);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.IsChildOf(transform))
                {
                    continue;
                }

                Vector3 offset = transform.TransformVector(_offset - _originOffset);

                transform.position = hit.point + offset;
                return;
            }
        }
    }
}