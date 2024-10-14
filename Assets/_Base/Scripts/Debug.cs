using System.Diagnostics;
using UnityEngine;

namespace FireRingStudio
{
    public static class Debug
    {
        #region Standard
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void Log(object message, Object context = null)
        {
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message, Object context = null)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message, Object context = null)
        {
            UnityEngine.Debug.LogError(message, context);
        }
        
        #endregion
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogNull(string name, Object context = null)
        {
            UnityEngine.Debug.LogWarning(name + " is null!", context);
        }
        
        #region LogNo
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogNo(string name, Object context = null)
        {
            UnityEngine.Debug.LogWarning("There is no " + name + "!");
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogNoWithLabel(string name, string labelName, string label, Object context = null)
        {
            UnityEngine.Debug.LogWarning("There is no " + name + " with " + labelName + " \"" + label + "\"!", context);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogNoInChildren(string name, Object context = null)
        {
            UnityEngine.Debug.LogWarning("There is no " + name + " in children!", context);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogNoInParent(string name, Object context = null)
        {
            UnityEngine.Debug.LogWarning("There is no " + name + " in parent!", context);
        }
        
        #endregion
        
        #region LogHasNo
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogHasNo(string sourceName, string targetName, Object context = null)
        {
            UnityEngine.Debug.LogWarning("The " + sourceName + " has no " + targetName + "!", context);
        }
        
        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        public static void LogHasNoWithLabel(string sourceName, string targetName, string labelName, string label, Object context = null)
        {
            UnityEngine.Debug.LogWarning("The " + sourceName + " has no " + targetName + " with " + labelName + " \"" + label + "\"!", context);
        }

        #endregion
    }
}