using UnityEngine;

namespace FireRingStudio
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }
}