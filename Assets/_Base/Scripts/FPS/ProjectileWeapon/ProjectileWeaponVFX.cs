using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [DisallowMultipleComponent]
    public class ProjectileWeaponVFX : ProjectileWeaponComponent
    {
        [SerializeField] private Transform _casingEjectionTransform;
        [SerializeField] private Transform _magazineEjectionTransform;
        
        [Header("Muzzle Light")]
        [SerializeField] private Transform _muzzleTransform;
        [SerializeField] private Light _muzzleLight;
        [ShowIf("@_muzzleLight != null")]
        [SerializeField, Range(0f, 0.2f)] private float _muzzleLightDuration = 0.05f;
        
        [Header("Tracer")]
        [SerializeField] private Transform _defaultTracerDirection;
        [HideIf("@_defaultTracerDirection != null")]
        [SerializeField] private string _defaultTracerDirectionId = "look";

        private Gun _gun;
        private float _disableMuzzleLightTime;

        private Transform DefaultTracerDirection
        {
            get
            {
                if (_defaultTracerDirection == null)
                {
                    _defaultTracerDirection = ComponentID.FindComponentWithID<Transform>(_defaultTracerDirectionId, true);
                }

                return _defaultTracerDirection;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            _gun = ProjectileWeapon as Gun;
        }

        private void Update()
        {
            if (GameManager.IsPaused || !IsEquipped)
            {
                return;
            }
            
            if (Time.time >= _disableMuzzleLightTime)
            {
                DisableMuzzleLight();
            }
        }

        private void OnDisable()
        {
            DisableMuzzleLight();
        }

        protected override void OnUse()
        {
            base.OnUse();
            PlayMuzzleFlash();
            EjectCasing();

            if (_gun != null && _gun.LastHit.collider != null)
            {
                PlayTracer(_gun.LastHit.point);
            }
            else if (DefaultTracerDirection != null)
            {
                Vector3 tracerTarget = DefaultTracerDirection.position + DefaultTracerDirection.forward * 10f;
                PlayTracer(tracerTarget);
            }
        }

        private void PlayMuzzleFlash()
        {
            if (Data == null || Data.MuzzleFlashPrefab == null || _muzzleTransform == null)
            {
                return;
            }

            Vector3 position = _muzzleTransform.position;
            Quaternion rotation = _muzzleTransform.rotation;
            rotation *= Quaternion.Euler(Vector3.right * -90f);
            
            Data.MuzzleFlashPrefab.GetInvisible(position, rotation, _muzzleTransform);
            
            EnableMuzzleLight();
        }

        private void EnableMuzzleLight()
        {
            if (_muzzleLight == null || _muzzleLight.gameObject.activeSelf)
            {
                return;
            }
            
            _muzzleLight.gameObject.SetActive(true);
            _disableMuzzleLightTime = Time.time + _muzzleLightDuration;
        }

        private void DisableMuzzleLight()
        {
            if (_muzzleLight != null)
            {
                _muzzleLight.gameObject.SetActive(false);
            }
        }

        private void PlayTracer(Vector3 targetPosition)
        {
            if (Data == null || Data.TracerPrefab == null || _muzzleTransform == null)
            {
                return;
            }

            Vector3 position = _muzzleTransform.position;
            PooledObject instance = Data.TracerPrefab.GetInvisible(position);

            instance.transform.LookAt(targetPosition, Vector3.forward);
        }

        private void EjectCasing()
        {
            if (Data == null || !Data.DropCasing || _casingEjectionTransform == null)
            {
                return;
            }

            ProjectileData projectileData = Data.ProjectileData;
            GameObject casingPrefab = projectileData != null ? projectileData.CasingPrefab : null;
            if (casingPrefab == null)
            {
                return;
            }

            Vector3 position = _casingEjectionTransform.position;
            Quaternion rotation = Quaternion.Euler(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30));
            
            PooledObject instance = casingPrefab.Get(position, rotation);
            instance.transform.localScale = Vector3.one * Data.CasingScale;
            
            Rigidbody casingRigidbody = ComponentCacheManager.GetComponent<Rigidbody>(instance.gameObject);

            if (casingRigidbody != null)
            {
                ApplyCasingVelocity(casingRigidbody);
            }
        }

        private void ApplyCasingVelocity(Rigidbody body)
        {
            if (Data == null)
            {
                return;
            }
            
            body.maxAngularVelocity = 10000f;

            Vector3 casingVelocity = new Vector3(
                Data.CasingVelocity.x * UnityEngine.Random.Range(0.75f, 1.15f),
                Data.CasingVelocity.y * UnityEngine.Random.Range(0.9f, 1.1f),
                Data.CasingVelocity.z * UnityEngine.Random.Range(0.85f, 1.15f));
            body.velocity = transform.TransformVector(casingVelocity);

            float spin = Data.CasingSpin;
            body.angularVelocity = new Vector3(
                UnityEngine.Random.Range(-spin, spin),
                UnityEngine.Random.Range(-spin, spin),
                UnityEngine.Random.Range(-spin, spin));
        }

        #region AnimationEvents

        private void EjectMagazine()
        {
            if (Data == null || !Data.DropEmptyMagazine || _magazineEjectionTransform == null)
            {
                return;
            }
            
            GameObject prefab = Data.MagazinePrefab;
            if (prefab == null)
            {
                return;
            }

            Vector3 position = _magazineEjectionTransform.position;
            PooledObject instance = prefab.GetInvisible(position, Quaternion.identity);

            if (instance.TryGetComponent(out Rigidbody body))
            {
                body.AddRelativeForce(Data.MagazineVelocity);
                body.AddRelativeTorque(Data.MagazineAngularVelocity);
            }
        }
        
        #endregion
    }
}