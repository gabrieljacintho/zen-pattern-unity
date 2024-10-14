using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(Gun))]
    public class RevolverCorrector : ProjectileWeaponCorrector
    {
        [Header("Revolver Corrector")]
        [SerializeField] private Transform _cylinderTransform;
        [SerializeField] private Vector3 _cylinderRotationOffset;
        [SerializeField] private float _cylinderSpeed = 10f;

        private Gun _gun;

        private Quaternion _defaultRotation;
        private Vector3 _currentRotation;
        private Vector3 _targetRotation;


        protected override void Awake()
        {
            base.Awake();
            _gun = GetComponent<Gun>();
            _gun.OnUseEvent.AddListener(RotateCylinder);

            if (_cylinderTransform != null)
            {
                _defaultRotation = _cylinderTransform.localRotation;
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            
            if (GameManager.IsPaused || _cylinderTransform == null)
            {
                return;
            }
            
            _currentRotation = Vector3.Lerp(_currentRotation, _targetRotation, _cylinderSpeed * Time.deltaTime);
            _cylinderTransform.Rotate(_currentRotation, Space.Self);
        }

        protected override void FillCartridges()
        {
            Fill(TotalQuantityOfAmmo);
            Restore();
        }

        private void RotateCylinder()
        {
            _targetRotation += _cylinderRotationOffset;
        }
        
        public void Restore()
        {
            if (_cylinderTransform != null)
            {
                _cylinderTransform.localRotation = _defaultRotation;
            }
            
            _currentRotation = Vector3.zero;
            _targetRotation = Vector3.zero;
        }
    }
}