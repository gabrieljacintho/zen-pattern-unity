using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FireRingStudio.Helpers
{
    public static class ObjectHelper
    {
        public static T[] FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return Object.FindObjectsOfType<MonoBehaviour>(includeInactive).OfType<T>().ToArray();
        }

        public static void DestroyOnLoad(GameObject targetObject)
        {
            SceneManager.MoveGameObjectToScene(targetObject, SceneManager.GetActiveScene());
        }
        
        public static T InstantiateComponent<T>() where T : Component
        {
            GameObject gameObject = new GameObject(typeof(T).Name);
            return gameObject.AddComponent<T>();
        }
    }
}