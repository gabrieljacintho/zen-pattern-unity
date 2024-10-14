using UnityEngine;

namespace FireRingStudio.FPS
{
    public class Recoil : MonoBehaviour
    {
        [SerializeField] private float _snappiness = 10f;
        [SerializeField] private float _returnSpeed = 2f;

        private Quaternion _targetRotation;
        private Quaternion _currentRotation;

        public float Snappiness
        {
            get => _snappiness;
            set => _snappiness = value;
        }
        public float ReturnSpeed
        {
            get => _returnSpeed;
            set => _returnSpeed = value;
        }
        public Vector3 CurrentRecoil => _currentRotation.eulerAngles;
        
        
        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            _targetRotation = Quaternion.Slerp(_targetRotation, Quaternion.Euler(Vector3.zero), _returnSpeed * Time.deltaTime);
            _currentRotation = Quaternion.Slerp(_currentRotation, _targetRotation, _snappiness * Time.deltaTime);
            
            transform.localRotation = _currentRotation;
        }

        public void AddRecoil(Vector3 force)
        {
            force = new Vector3(force.x, UnityEngine.Random.Range(-force.y, force.y), UnityEngine.Random.Range(-force.z, force.z));
            _targetRotation *= Quaternion.Euler(force);
        }
    }
}
