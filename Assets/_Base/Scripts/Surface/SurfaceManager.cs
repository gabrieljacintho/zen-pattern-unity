using FireRingStudio.Pool;
using System.Collections.Generic;
using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using FireRingStudio.Patterns;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Runtime.ConstrainedExecution;

namespace FireRingStudio.Surface
{
    public class SurfaceManager : PersistentSingleton<SurfaceManager>
    {
        [FolderPath(ParentFolder = "Assets/Resources")]
        [SerializeField] private string _dataPath;
        [SerializeField] private SurfaceData _defaultSurface;

        public static List<SurfaceData> SurfacesDatabase { get; private set; } = new List<SurfaceData>();
        public static List<Terrain> Terrains { get; private set; } = new List<Terrain>();
        
        private static readonly Dictionary<GameObject, SurfaceData> s_surfaceDataByObject = new();
        private static readonly Dictionary<GameObject, Surface> s_surfaceByObject = new();
        private static readonly Dictionary<GameObject, Renderer> s_rendererByObject = new();
        private static readonly Dictionary<Terrain, float[,,]> s_alphamapsByTerrain = new();


        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                return;
            }

            SurfacesDatabase = ResourcesHelper.LoadAll<SurfaceData>(_dataPath).ToList();
            Terrains = FindObjectsOfType<Terrain>().ToList();

            foreach (Terrain terrain in Terrains)
            {
                TerrainData terrainData = terrain.terrainData;
                float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
                s_alphamapsByTerrain.Add(terrain, alphamaps);
            }
        }

        public static PooledObject SpawnEffect(SurfaceData surfaceData, SurfaceEffectType type, string id, Vector3 position,
            Quaternion rotation = default, Transform parent = null)
        {
            GameObject effectPrefab = surfaceData.GetEffectPrefab(id, type);
            if (effectPrefab == null)
            {
                return null;
            }

            return effectPrefab.Get(position, rotation, parent);
        }

        public static PooledObject SpawnEffect(RaycastHit hit, SurfaceEffectType type, string id, bool canParent = true)
        {
            if (!TryGetSurfaceData(hit, out SurfaceData surfaceData))
            {
                if (Instance != null)
                {
                    surfaceData = Instance._defaultSurface;
                }
            }

            if (surfaceData == null)
            {
                return null;
            }

            Vector3 position = hit.point;
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            Transform parent = canParent ? hit.transform : null;

            return SpawnEffect(surfaceData, type, id, position, rotation, parent);
        }
        
        public static PooledObject SpawnEffect(GameObject gameObject, SurfaceEffectType type, string id)
        {
            if (!TryGetObjectSurfaceData(gameObject, out SurfaceData surfaceData))
            {
                if (Instance != null)
                {
                    surfaceData = Instance._defaultSurface;
                }
            }
            
            if (surfaceData == null)
            {
                return null;
            }

            Vector3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;
            Transform parent = gameObject.transform;

            return SpawnEffect(surfaceData, type, id, position, rotation, parent);
        }

        #region Getters
        
        public static bool TryGetSurfaceData(RaycastHit hit, out SurfaceData surfaceData)
        {
            if (TryGetTerrainSurfaceData(hit, out surfaceData))
            {
                return true;
            }

            if (TryGetObjectSurfaceData(hit.transform.gameObject, out surfaceData))
            {
                return true;
            }
            
            return false;
        }

        private static bool TryGetObjectSurfaceData(GameObject gameObject, out SurfaceData surfaceData)
        {
            if (s_surfaceDataByObject.TryGetValue(gameObject, out surfaceData))
            {
                return surfaceData != null;
            }

            TryGetSurfaceComponentData(gameObject, out surfaceData);

            if (surfaceData == null)
            {
                TryGetRendererSurfaceData(gameObject, out surfaceData);
            }
            
            s_surfaceDataByObject.Add(gameObject, surfaceData);

            return surfaceData != null;
        }

        private static bool TryGetSurfaceComponentData(GameObject gameObject, out SurfaceData surfaceData)
        {
            surfaceData = null;

            if (!s_surfaceByObject.TryGetValue(gameObject, out Surface surface))
            {
                surface = gameObject.GetComponent<Surface>();
                s_surfaceByObject.Add(gameObject, surface);
            }
            
            if (surface != null)
            {
                surfaceData = surface.Data;
            }
            
            return surfaceData != null;
        }

        private static bool TryGetRendererSurfaceData(GameObject gameObject, out SurfaceData surfaceData)
        {
            surfaceData = null;
            
            if (SurfacesDatabase == null || SurfacesDatabase.Count == 0)
            {
                return false;
            }
            
            if (!s_rendererByObject.TryGetValue(gameObject, out Renderer renderer))
            {
                renderer = gameObject.GetComponentInChildren<Renderer>(true);
                s_rendererByObject.Add(gameObject, renderer);
            }
            
            if (renderer == null || renderer.materials == null)
            {
                return false;
            }
            
            foreach (Material material in renderer.materials)
            {
                if (material == null)
                {
                    continue;
                }
                        
                surfaceData = SurfacesDatabase.Find(x => x != null && x.HasTexture(material.mainTexture));
                if (surfaceData != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetTerrainSurfaceData(RaycastHit hit, out SurfaceData surfaceData)
        {
            surfaceData = null;

            if (SurfacesDatabase == null || SurfacesDatabase.Count == 0 || Terrains == null)
            {
                return false;
            }
            
            Terrain terrain = Terrains.Find(terrain => terrain != null && terrain.transform == hit.transform);
            if (terrain == null || terrain.terrainData.terrainLayers.Length == 0)
            {
                return false;
            }
            
            int textureIndex = GetTerrainTextureIndex(terrain, hit.point);
            Texture texture = terrain.terrainData.terrainLayers[textureIndex].diffuseTexture;
            
            surfaceData = SurfacesDatabase.Find(surfaceData => surfaceData != null && surfaceData.HasTexture(texture));

            return surfaceData != null;
        }
        
        private static int GetTerrainTextureIndex(Terrain terrain, Vector3 hitPoint)
        {
            float[,,] alphamaps = GetAlphamaps(terrain);
            Vector2Int alphamapCoordinate = ConvertToAlphamapCoordinate(terrain, hitPoint);

            int mostDominantTextureIndex = 0;
            float greatestTextureWeight = float.MinValue;

            int textureCount = alphamaps.GetLength(2);
            for (int textureIndex = 0; textureIndex < textureCount; textureIndex++)
            {
                float textureWeight = alphamaps[alphamapCoordinate.y, alphamapCoordinate.x, textureIndex];

                if (textureWeight > greatestTextureWeight)
                {
                    greatestTextureWeight = textureWeight;
                    mostDominantTextureIndex = textureIndex;
                }
            }

            return mostDominantTextureIndex;
        }

        private static float[,,] GetAlphamaps(Terrain terrain)
        {
            if (!s_alphamapsByTerrain.TryGetValue(terrain, out float[,,] alphamaps))
            {
                TerrainData terrainData = terrain.terrainData;
                alphamaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
                s_alphamapsByTerrain.Add(terrain, alphamaps);
            }

            return alphamaps;
        }

        private static Vector2Int ConvertToAlphamapCoordinate(Terrain terrain, Vector3 hitPoint)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPosition = terrain.transform.position;
            Vector3 relativePosition = hitPoint - terrainPosition;

            int x = Mathf.RoundToInt(relativePosition.x / terrainData.size.x * terrainData.alphamapWidth);
            int y = Mathf.RoundToInt(relativePosition.z / terrainData.size.z * terrainData.alphamapHeight);

            return new Vector2Int(x, y);
        }

        #endregion
    }
}