using FireRingStudio.Extensions;
using FireRingStudio.Surface;
using FireRingStudio.Vitals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    public class Gun : ProjectileWeaponBase
    {
        [Header("Gun")]
        [SerializeField] protected Transform _lookTransform;
        [HideIf("@_lookTransform != null")]
        [SerializeField] protected string _lookTransformId = "look";

        public RaycastHit LastHit { get; private set; }
        private Transform LookTransform
        {
            get
            {
                if (_lookTransform == null)
                {
                    _lookTransform = ComponentID.FindComponentWithID<Transform>(_lookTransformId, true);
                }
                
                return _lookTransform;
            }
        }
        
        protected override void FireProjectile()
        {
            if (!TryGetFireHit(out RaycastHit hit))
            {
                LastHit = default;
                return;
            }

            LastHit = hit;
            
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                ApplyDamage(damageable, hit.distance);
            }
            
            if (Data.PushLayerMask.Contains(hit.transform.gameObject.layer) && hit.rigidbody != null)
            {
                Push(hit.rigidbody, hit.point);
            }

            ProjectileData projectileData = Data.ProjectileData;
            if (projectileData != null)
            {
                SurfaceManager.SpawnEffect(hit, projectileData.ImpactEffectType, projectileData.ImpactEffectId, false);
            }
        }
        
        private bool TryGetFireHit(out RaycastHit hit)
        {
            hit = default;
            
            if (Data == null || LookTransform == null)
            {
                return false;
            }
            
            Vector3 origin = LookTransform.position;
            float maxDistance = Data.MaxDistance;

            Vector3 direction = GetFireDirection(LookTransform, CurrentPrecision.Radius, maxDistance);
            Ray ray = new Ray(origin, direction);

            DamageParameters damageParameters = Data.DamageParameters;
            int layerMask = damageParameters.LayerMask;
            QueryTriggerInteraction queryTriggerInteraction = damageParameters.TriggerInteraction;
            
            return UnityEngine.Physics.Raycast(ray, out hit, maxDistance, layerMask, queryTriggerInteraction);
        }

        public static Vector3 GetFireDirection(Transform directionTransform, float precisionRadius, float maxDistance)
        {
            Vector3 origin = directionTransform.position;
            Vector3 maxHitPoint = origin + directionTransform.forward * maxDistance;
            
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * precisionRadius;
            maxHitPoint += directionTransform.right * randomPoint.x;
            maxHitPoint += directionTransform.up * randomPoint.y;

            return (maxHitPoint - origin).normalized;
        }

        private void ApplyDamage(IDamageable damageable, float impactDistance)
        {
            if (Data == null)
            {
                return;
            }

            float damageAmount = GetDamageAmount(Data.DamageParameters, impactDistance);
            
            Damage damage = new(DamageType.Impact, damageAmount, transform.position);

            damageable.TakeDamage(damage);
        }

        private float GetDamageAmount(DamageParameters parameters, float impactDistance)
        {
            float value = parameters.GetRandomDamageAmount();

            if (Data == null)
            {
                return value;
            }
            
            if (Data.FireMode != FireMode.Burst)
            {
                value /= Data.ProjectilesPerShot;
            }

            float time = Mathf.Clamp(impactDistance / Data.MaxDistance, 0f, 1f);
            value *= Data.DamageCurve.Evaluate(time);

            return value;
        }
        
        private void Push(Rigidbody body, Vector3 position)
        {
            if (LookTransform != null && Data != null && Data.PushForce != 0f)
            {
                body.Push(LookTransform.forward, Data.PushForce, position);
            }
        }
    }
}