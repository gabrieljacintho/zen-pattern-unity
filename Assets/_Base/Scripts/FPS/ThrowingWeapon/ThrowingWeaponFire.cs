using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.FPS.Equipment;
using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.Inventory;
using FireRingStudio.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.ThrowingWeapon
{
    [DisallowMultipleComponent]
    public class ThrowingWeaponFire : EquipmentComponent
    {
        [SerializeField] private GameObject _meshObject;
        [SerializeField] private Transform _projectileTransform;
        [SerializeField] private ThrowObject _throwObject;
        [ShowIf("@_throwObject == null")]
        [SerializeField] private string _throwObjectId;

        private ProjectileWeaponBase _projectileWeapon;
        private PooledObject _projectileInstance;
        private int _defaultProjectileLayer;
        
        private ThrowObject ThrowObject
        {
            get
            {
                if (_throwObject == null)
                {
                    _throwObject = ComponentID.FindComponentWithID<ThrowObject>(_throwObjectId, true);
                }

                return _throwObject;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _projectileWeapon = Equipment as ProjectileWeaponBase;
        }

        protected override void OnEquip()
        {
            base.OnEquip();

            if (_projectileWeapon != null)
            {
                ItemPack ammoClip = _projectileWeapon.AmmoClip;
                SetActiveMeshObject(ammoClip != null && ammoClip.Size > 0);
            }
            else
            {
                SetActiveMeshObject(true);
            }
        }

        public PooledObject InstantiateProjectile(GameObject prefab)
        {
            if (_projectileInstance != null)
            {
                return _projectileInstance;
            }

            SetActiveMeshObject(false);

            _projectileInstance = _projectileTransform != null ? prefab.GetInvisible(_projectileTransform) : prefab.GetInvisible(transform.position);

            GameObject projectileObject = _projectileInstance.gameObject;
            
            _defaultProjectileLayer = projectileObject.layer;
            if (_projectileTransform != null)
            {
                projectileObject.SetLayerRecursively(_projectileTransform.gameObject.layer);
            }

            Rigidbody body = ComponentCacheManager.GetComponent<Rigidbody>(projectileObject);

            if (body != null)
            {
                body.isKinematic = true;
            }

            return _projectileInstance;
        }

        public void ThrowProjectile(ThrowParameters parameters, float precisionRadius = 0f, float maxDistance = 0f)
        {
            if (_projectileInstance == null)
            {
                Debug.LogError("Projectile instance is null!", this);
                return;
            }
            
            SetActiveMeshObject(false);
            
            GameObject projectileObject = _projectileInstance.gameObject;
            projectileObject.SetLayerRecursively(_defaultProjectileLayer);

            Rigidbody body = ComponentCacheManager.GetComponent<Rigidbody>(projectileObject);
            
            if (body != null)
            {
                body.isKinematic = false;
            }

            if (ThrowObject != null)
            {
                if (_projectileWeapon != null && _projectileWeapon.Data != null)
                {
                    parameters.Force *= _projectileWeapon.Data.MaxDistance;
                }

                ThrowObject.ThrowInstance(projectileObject, parameters, precisionRadius, maxDistance);
            }

            _projectileInstance = null;
        }

        public PooledObject ThrowProjectile(GameObject prefab, ThrowParameters parameters, float precisionRadius = 0f, float maxDistance = 0f)
        {
            PooledObject projectileInstance = InstantiateProjectile(prefab);
            ThrowProjectile(parameters, precisionRadius, maxDistance);
            return projectileInstance;
        }
        
        public void SetActiveMeshObject(bool value)
        {
            if (_meshObject != null)
            {
                _meshObject.SetActive(value);
            }
        }
        
        #region AnimationEvents

        private void EnableMesh()
        {
            SetActiveMeshObject(true);
        }
        
        #endregion
    }
}