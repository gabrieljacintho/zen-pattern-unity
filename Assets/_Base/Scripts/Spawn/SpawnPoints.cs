using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Pool;
using UnityEngine;

namespace FireRingStudio.Spawn
{
    public class SpawnPoints : Spawner
    {
        [Header("Extra Settings")]
        [Tooltip("If null the component Transform is used.")]
        [SerializeField] private Transform _spawnPointsRoot;

        private Transform SpawnPointsRoot => _spawnPointsRoot != null ? _spawnPointsRoot.transform : transform;


        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (_avoidanceRadius > 0f)
            {
                Gizmos.color = Color.white;
                
                Transform[] spawnPoints = SpawnPointsRoot.GetChildren();

                foreach (Transform spawnPoint in spawnPoints)
                {
                    Gizmos.DrawWireSphere(spawnPoint.position, _avoidanceRadius);
                }
            }
        }
        
        protected override bool TryGetSpawnPoint(GameObject prefab, bool invisible, out Vector3 point, out Quaternion rotation, out List<PooledObject> releasedObjects)
        {
            point = Vector3.zero;
            rotation = Quaternion.identity;

            List<Transform> spawnPoints = GetValidSpawnPoints(invisible, out releasedObjects);
            if (spawnPoints.Count == 0)
            {
                return false;
            }

            int index = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[index];

            point = spawnPoint.position;
            rotation = spawnPoint.rotation;

            return true;
        }

        private List<Transform> GetValidSpawnPoints(bool invisible, out List<PooledObject> releasedObjects)
        {
            List<Transform> spawnPoints = SpawnPointsRoot.GetChildren().ToList();
            releasedObjects = new List<PooledObject>();

            Camera currentCamera = Camera.current;
            
            List<Transform> validSpawnPoints = new List<Transform>();
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (!spawnPoint.gameObject.activeSelf)
                {
                    continue;
                }
                
                float distance = Vector3.Distance(transform.position, spawnPoint.position);
                if (distance < MinSpawnDistance || distance > MaxSpawnDistance)
                {
                    continue;
                }

                if (invisible)
                {
                    if (currentCamera != null && spawnPoint.position.IsVisibleByCamera(currentCamera))
                    {
                        continue;
                    }
                }

                if (!CanSpawnAt(spawnPoint.position, out releasedObjects))
                {
                    continue;
                }
                
                validSpawnPoints.Add(spawnPoint);
            }

            return validSpawnPoints;
        }
    }
}