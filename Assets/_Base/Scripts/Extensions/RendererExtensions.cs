using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class RendererExtensions
    {
        public static void DestroyMaterials(this Renderer renderer)
        {
            Material[] materials = renderer.materials;
            if (materials == null)
            {
                return;
            }
            
            foreach (Material material in materials)
            {
                if (material != null)
                {
                    Object.Destroy(material);
                }
            }
        }

        public static void SetEnabled(this List<Renderer> renderers, bool value)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = value;
            }
        }
    }
}