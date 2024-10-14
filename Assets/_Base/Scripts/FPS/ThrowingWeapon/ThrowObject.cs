using FireRingStudio.Cache;
using FireRingStudio.Extensions;
using FireRingStudio.FPS.ProjectileWeapon;
using FireRingStudio.Pool;
using FireRingStudio.Spawn;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.ThrowingWeapon
{
    public class ThrowObject : MonoBehaviour
    {
        [Tooltip("If null the component Transform is used.")]
        [SerializeField] private Transform _direction;
        [SerializeField] private Spawner _spawner;
        [HideIf("@_spawner != null")]
        [SerializeField] private string _spawnerId;

        [Header("Settings")]
        [SerializeField] private ThrowParameters _parameters = new ThrowParameters(9f, 0.01f, 1f);
        [SerializeField] private float _precisionRadius;
        [SerializeField] private float _maxDistance;

        private Transform Direction => _direction != null ? _direction : transform;


        private void Awake()
        {
            if (_spawner == null && !string.IsNullOrEmpty(_spawnerId))
            {
                _spawner = ComponentID.FindComponentWithID<Spawner>(_spawnerId, true);
            }
        }

        public PooledObject Throw(GameObject prefab, ThrowParameters parameters, float precisionRadius = 0f, float maxDistance = 0f)
        {
            PooledObject instance;
            if (_spawner != null)
            {
                instance = _spawner.Spawn(prefab, Direction.position, Direction.rotation, true);
            }
            else
            {
                instance = prefab.GetInvisible(Direction.position, Direction.rotation);
            }

            ThrowInstance(instance.gameObject, parameters, precisionRadius, maxDistance);

            return instance;
        }
        
        public PooledObject Throw(GameObject prefab)
        {
            return Throw(prefab, _parameters, _precisionRadius, _maxDistance);
        }
        
        public void ThrowInstance(GameObject instance, ThrowParameters parameters, float precisionRadius = 0f, float maxDistance = 0f)
        {
            Vector3 throwingPoint = GetThrowingPoint(parameters, out bool hitted);
            
            instance.transform.SetParent(null);
            instance.transform.SetPositionAndRotation(throwingPoint, Direction.rotation);
            
            if (!hitted)
            {
                Rigidbody body = ComponentCacheManager.GetComponent<Rigidbody>(instance);
                if (body != null)
                {
                    body.ResetVelocity();
                    AddForce(body, parameters, precisionRadius, maxDistance);
                    AddTorque(body, parameters);
                }
            }
        }
        
        public void ThrowInstance(GameObject instance)
        {
            ThrowInstance(instance, _parameters, _precisionRadius, _maxDistance);
        }
        
        private Vector3 GetThrowingPoint(ThrowParameters parameters, out bool hitted)
        {
            Vector3 throwingPoint = Direction.position;
            Ray ray = new Ray(Direction.position, Direction.forward);
            
            if (UnityEngine.Physics.Raycast(ray, out RaycastHit hit, parameters.Distance, parameters.ObstacleLayers))
            {
                throwingPoint += Direction.forward * (hit.distance - 0.2f);
                hitted = true;
            }
            else
            {
                throwingPoint += Direction.forward * parameters.Distance;
                hitted = false;
            }

            return throwingPoint;
        }

        private void AddForce(Rigidbody body, ThrowParameters parameters, float precisionRadius = 0f, float maxDistance = 10f)
        {
            Vector3 direction;
            if (precisionRadius > 0f)
            {
                direction = Gun.GetFireDirection(transform, precisionRadius, maxDistance);
            }
            else
            {
                direction = transform.forward;
            }
            
            body.AddForce(direction * parameters.Force, ForceMode.Impulse);
        }
        
        private void AddTorque(Rigidbody body, ThrowParameters parameters)
        {
            float torqueForce = UnityEngine.Random.Range(-parameters.TorqueRange, parameters.TorqueRange);
            body.AddTorque(transform.forward * torqueForce, ForceMode.Impulse);
        }
    }
}