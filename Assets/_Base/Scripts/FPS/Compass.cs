using Sirenix.OdinInspector;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace FireRingStudio.FPS
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private Transform _northIndicator;
        [SerializeField] private Transform _direction;
        [HideIf("@_direction != null")]
        [SerializeField] private string _directionId = "player";

        private static Vector3 NorthDirection => Vector3.forward;

        private Transform Direction
        {
            get
            {
                if (_direction == null)
                {
                    _direction = ComponentID.FindComponentWithID<Transform>(_directionId, true);
                }
                
                return _direction;
            }
        }
        

        private void LateUpdate()
        {
            if (GameManager.IsPaused || _northIndicator == null)
            {
                return;
            }

            float angle = -Vector3.SignedAngle(Direction.forward, NorthDirection, Vector3.up);
            Vector3 rotation = Vector3.Scale(new Vector3(angle, angle, angle), Vector3.forward.normalized);
            
            _northIndicator.localRotation = Quaternion.Euler(_northIndicator.localEulerAngles + rotation);
        }
    }
}