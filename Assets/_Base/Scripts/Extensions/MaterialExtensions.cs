using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class MaterialExtensions 
    {
        private static readonly int s_colorPropertyID = Shader.PropertyToID("_Color");
        private static readonly int s_emissionColorPropertyID = Shader.PropertyToID("_EmissionColor");


        public static Color GetColor(this Material material)
        {
            return material.GetColor(s_colorPropertyID);
        }
        
        public static void SetColor(this Material material, Color value)
        {
            material.SetColor(s_colorPropertyID, value);
        }

        public static float GetAlpha(this Material material)
        {
            return material.GetColor().a;
        }
        
        public static float GetAlpha(this Material material, string colorPropertyName)
        {
            return material.GetColor(colorPropertyName).a;
        }
        
        public static float GetAlpha(this Material material, int colorPropertyID)
        {
            return material.GetColor(colorPropertyID).a;
        }
        
        public static void SetAlpha(this Material material, float value)
        {
            Color color = material.GetColor();
            color.a = value;
            
            material.SetColor(color);
        }
        
        public static void SetAlpha(this Material material, string colorPropertyName, float value)
        {
            Color color = material.GetColor(colorPropertyName);
            color.a = value;
            
            material.SetColor(colorPropertyName, color);
        }
        
        public static void SetAlpha(this Material material, int colorPropertyID, float value)
        {
            Color color = material.GetColor(colorPropertyID);
            color.a = value;
            
            material.SetColor(colorPropertyID, color);
        }
        
        public static Color GetEmissionColor(this Material material)
        {
            return material.GetColor(s_emissionColorPropertyID);
        }
        
        public static void SetEmissionColor(this Material material, Color value)
        {
            material.SetColor(s_emissionColorPropertyID, value);
        }
    }
}