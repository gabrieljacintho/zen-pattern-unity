using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.LevelManagement
{
    public class LevelObject : MonoBehaviour
    {
        [SerializeField] private bool _selfDestroyOnUnload = true;

        [Space]
        public UnityEvent OnLoadEvent;
        public UnityEvent OnUnloadEvent;
        public UnityEvent OnCompleteEvent;


        public void OnLoad()
        {
            OnLoadEvent?.Invoke();
        }

        public void OnUnload()
        {
            if (_selfDestroyOnUnload)
            {
                Destroy(gameObject);
            }

            OnUnloadEvent?.Invoke();
        }

        public void OnComplete()
        {
            OnCompleteEvent?.Invoke();
        }
    }
}