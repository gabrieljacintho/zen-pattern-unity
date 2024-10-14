using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.Physics
{
    [RequireComponent(typeof(Collider))]
    public class Sensor : Detector
    {
        [Header("Sensor")]
        [SerializeField, Range(0f, 360f)] private float _angle = 360f;
        [SerializeField, Min(1)] private int _updateInterval = 1;

        private bool CanUpdate => Time.frameCount % _updateInterval == 0;


        private void OnTriggerStay(Collider other)
        {
            if (!isActiveAndEnabled || !CanUpdate)
            {
                return;
            }
            
            bool wasDetecting = IsDetecting;

            if (CanDetect(other))
            {
                if (!_colliders.Contains(other))
                {
                    _colliders.Add(other);
                }
            }
            else
            {
                _colliders.Remove(other);
            }

            TryInvokeEvents(wasDetecting);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            bool wasDetecting = IsDetecting;

            _colliders.Remove(other);

            TryInvokeEvents(wasDetecting);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (_angle <= 0 || _angle >= 360f)
            {
                return;
            }
            
            Gizmos.color = Color.white;
            GizmosHelper.DrawAngle(transform, _angle);
        }

        public override bool CanDetect(Collider targetCollider)
        {
            return transform.CheckAngle(targetCollider.transform.position, _angle) && base.CanDetect(targetCollider);
        }
    }
}