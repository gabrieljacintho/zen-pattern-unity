using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using FireRingStudio.Pool;
using FireRingStudio.Surface;
using FireRingStudio.Vitals;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [RequireComponent(typeof(Rigidbody), typeof(AttachRigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileData _data;
        [SerializeField] private float _spawnCollectableItemDelay = 1f;
        [SerializeField] private bool _releaseToPoolOnApplyDamage;

        private Rigidbody _rigidbody;
        private AttachRigidbody _attachRigidbody;
        private ProjectileWeaponData _projectileWeaponData;
        private IDamageable _damaged;
        private WaitForSeconds _waitForSpawnCollectableItemDelay;

        private float _time;
        private bool _impacted;
        private bool _collectableItemSpawnStarted;

        private Rigidbody Rigidbody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = GetComponent<Rigidbody>();
                }

                return _rigidbody;
            }
        }
        private AttachRigidbody AttachRigidbody
        {
            get
            {
                if (_attachRigidbody == null)
                {
                    _attachRigidbody = GetComponent<AttachRigidbody>();
                }

                return _attachRigidbody;
            }
        }
        private float Speed
        {
            get
            {
                if (_time < 0.01f)
                {
                    return _projectileWeaponData.ThrowParameters.Force * _projectileWeaponData.MaxDistance;
                }

                return Rigidbody.velocity.magnitude;
            }
        }
        
        [Space]
        [InspectorName("On Impact")] public UnityEvent OnImpactEvent;


        private void Awake()
        {
            if (_spawnCollectableItemDelay > 0f)
            {
                _waitForSpawnCollectableItemDelay = new WaitForSeconds(_spawnCollectableItemDelay);
            }
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            _time += Time.deltaTime;

            UpdateProjectile();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            OnImpactEnter(other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnImpactEnter(collision.collider);
        }

        private void OnDisable()
        {
            if (gameObject.activeSelf && UpdateManager.Instance != null)
            {
                UpdateManager.Instance.DoOnNextFrame(() => transform.parent = null);
            }
        }

        private void OnImpactEnter(Collider hitCollider)
        {
            if (_impacted)
            {
                return;
            }
            
            if (!PhysicsHelper.CanDetect(hitCollider.gameObject, _data.LayerMask))
            {
                return;
            }

            if (CheckForSurfaces(out RaycastHit hit))
            {
                OnImpact(hit);
            }
            else
            {
                OnImpact(hitCollider);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_data == null)
            {
                return;
            }
            
            Vector3 from = transform.position;
            Vector3 to = from + transform.forward * _data.MaxDistance;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(from, to);
        }

        public void Initialize(ProjectileWeaponData projectileWeaponData)
        {
            _projectileWeaponData = projectileWeaponData;
            _damaged = null;
            _impacted = false;
            _collectableItemSpawnStarted = false;
            _time = 0f;
            UpdateProjectile();
        }

        private void UpdateProjectile()
        {
            if (!_impacted)
            {
                if (CheckForSurfaces(out RaycastHit hit))
                {
                    OnImpact(hit);
                }
            }
            
            if (gameObject.activeSelf && _impacted && !_collectableItemSpawnStarted)
            {
                if (_damaged == null || _damaged.CurrentHealth <= 0f)
                {
                    if (UpdateManager.Instance != null)
                    {
                        UpdateManager.Instance.StartCoroutine(SpawnCollectableItem());
                    }
                    _collectableItemSpawnStarted = true;
                }
            }
        }

        private bool CheckForSurfaces(out RaycastHit hit)
        {
            if (_data == null)
            {
                hit = default;
                return false;
            }
            
            Ray ray = new Ray(transform.position, transform.forward);
            return UnityEngine.Physics.Raycast(ray, out hit, _data.MaxDistance, _data.LayerMask, _data.QueryTriggerInteraction);
        }

        private void OnImpact(RaycastHit hit)
        {
            if (hit.rigidbody != null)
            {
                Push(hit.rigidbody, hit.point);
            }

            Penetrate(hit.point);
            
            if (_data != null)
            {
                SurfaceManager.SpawnEffect(hit, _data.ImpactEffectType, _data.ImpactEffectId);
            }
            
            OnImpact(hit.collider);
            
            Rigidbody.ResetVelocity();
        }

        private void OnImpact(Collider hitCollider)
        {
            _impacted = true;

            _damaged = ComponentCacheManager.GetComponent<IDamageable>(hitCollider.gameObject);
            if (_damaged != null)
            {
                ApplyDamage(_damaged, Speed);
                transform.SetParent(hitCollider.transform);

                if (_releaseToPoolOnApplyDamage)
                {
                    gameObject.ReleaseToPool();
                }
            }

            AttachRigidbody.TargetCollider = hitCollider;

            OnImpactEvent?.Invoke();
        }

        private void ApplyDamage(IDamageable damageable, float speed)
        {
            if (_projectileWeaponData == null)
            {
                return;
            }

            DamageParameters parameters = _projectileWeaponData.DamageParameters;
            float damageAmount = GetDamageAmount(parameters, speed);
            
            Damage damage = new Damage(parameters.Type, damageAmount, transform.position);
            
            damageable.TakeDamage(damage);
        }

        private float GetDamageAmount(DamageParameters parameters, float speed)
        {
            float damageAmount = parameters.GetRandomDamageAmount();
            
            if (_projectileWeaponData == null)
            {
                return damageAmount;
            }

            float time = 1f - Mathf.Clamp(speed / _projectileWeaponData.MaxDistance, 0f, 1f);
            damageAmount *= _projectileWeaponData.DamageCurve.Evaluate(time);

            return damageAmount;
        }

        private void Push(Rigidbody body, Vector3 position)
        {
            if (_projectileWeaponData != null && _projectileWeaponData.PushForce != 0f)
            {
                body.Push(transform.forward, _projectileWeaponData.PushForce, position);
            }
        }

        private void Penetrate(Vector3 position)
        {
            float penetrationOffset = _data != null ? _data.PenetrationOffset : 0f;
            transform.position = position + transform.forward * penetrationOffset;
        }

        private IEnumerator SpawnCollectableItem()
        {
            if (_data == null || _data.Prefab == null)
            {
                yield break;
            }

            if (_spawnCollectableItemDelay > 0f)
            {
                yield return _waitForSpawnCollectableItemDelay;
            }
            
            PooledObject instance = _data.Prefab.GetInvisible(transform.position, transform.rotation);

            /*Vector3 scale = parent.localScale;
            if (scale != Vector3.one) // TODO: Optimize
            {
                GameObject container = new GameObject("Container");
                container.transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
                container.transform.SetParent(parent, false);
                parent = container.transform;
            }*/

            instance.transform.SetParent(transform.parent);

            AttachRigidbody attachRigidbody = ComponentCacheManager.GetComponent<AttachRigidbody>(instance.gameObject);
            if (attachRigidbody != null)
            {
                attachRigidbody.TargetCollider = AttachRigidbody.TargetCollider;
            }
            
            gameObject.ReleaseToPool();
        }
    }
}