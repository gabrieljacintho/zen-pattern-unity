using FireRingStudio.Extensions;
using FireRingStudio.Physics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Spawn
{
    public class SpawnPointsGenerator : MonoBehaviour
    {
        [SerializeField] private int _xCount = 512;
        [SerializeField] private float _xOffset = 1f;
        [SerializeField] private int _zCount = 512;
        [SerializeField] private float _zOffset = 1f;


        [Button]
        public void DestroyChildren()
        {
            transform.DestroyChildrenImmediate();
        }

        [Button]
        public void Generate()
        {
            Vector3 position = transform.position;
            int i = 0;

            for (int x = 0; x < _xCount; x++)
            {
                Vector3 rowPosition = position;
                for (int z = 0; z < _zCount; z++)
                {
                    i++;

                    GameObject spawnPoint = new GameObject("SpawnPoint_" + i);
                    spawnPoint.transform.position = rowPosition;
                    spawnPoint.transform.parent = transform;

                    RaycastOffset raycastOffset = spawnPoint.AddComponent<RaycastOffset>();
                    raycastOffset.Setup(Vector3.zero, -Vector3.up, LayerMask.NameToLayer("Terrain"), QueryTriggerInteraction.Ignore, Vector3.up * 0.05f);

                    rowPosition.z += _zOffset;
                }

                position.x += _xOffset;
            }
        }
    }
}