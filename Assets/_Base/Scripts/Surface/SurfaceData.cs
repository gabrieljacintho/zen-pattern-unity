using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Surface
{
    public enum SurfaceEffectType
    {
        Footstep,
        FallImpact,
        BulletImpact,
        Slash,
        Stab
    }

    [CreateAssetMenu(menuName = "FireRing Studio/Surface Data", fileName = "New Surface Data")]
    public class SurfaceData : ScriptableObject
    {
        [FormerlySerializedAs("textures")]
        [SerializeField] private List<Texture> _textures;

        [Space]
        [SerializeField] private List<KeyValue<string, GameObject>> _footstepEffectPrefabs;
        [SerializeField] private List<KeyValue<string, GameObject>> _fallImpactEffectPrefabs;
        [SerializeField] private List<KeyValue<string, GameObject>> _bulletImpactEffectPrefabs;
        [SerializeField] private List<KeyValue<string, GameObject>> _slashEffectPrefabs;
        [SerializeField] private List<KeyValue<string, GameObject>> _stabEffectPrefabs;


        public GameObject GetEffectPrefab(string id, SurfaceEffectType type)
        {
            switch (type)
            {
                case SurfaceEffectType.Footstep:
                    return FindEffectPrefab(_footstepEffectPrefabs, id);

                case SurfaceEffectType.FallImpact:
                    return FindEffectPrefab(_fallImpactEffectPrefabs, id);

                case SurfaceEffectType.BulletImpact:
                    return FindEffectPrefab(_bulletImpactEffectPrefabs, id);

                case SurfaceEffectType.Slash:
                    return FindEffectPrefab(_slashEffectPrefabs, id);

                case SurfaceEffectType.Stab:
                    return FindEffectPrefab(_stabEffectPrefabs, id);

                default:
                    return null;
            }
        }

        public bool HasTexture(Texture texture)
        {
            return _textures != null && _textures.Contains(texture);
        }

        private static GameObject FindEffectPrefab(List<KeyValue<string, GameObject>> prefabs, string id = null)
        {
            GameObject prefab = prefabs?.FindValueWithID(id);
            if (prefab == null && !string.IsNullOrEmpty(id))
            {
                prefab = prefabs?.FindValueWithID(null);
            }

            return prefab;
        }
    }
}