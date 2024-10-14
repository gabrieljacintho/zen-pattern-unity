using FireRingStudio.Patterns;
using UnityEngine;

namespace FireRingStudio
{
    public class UpdateManager : PersistentSingleton<UpdateManager>
    {
        [SerializeField, Min(1)] private int _interval = 1;

        public static int FixedUpdateCount { get; private set; }

        private bool CanFixedUpdate => FixedUpdateCount % _interval == 0;
        private bool CanUpdate => Time.frameCount % _interval == 0;
        
        public delegate void UpdateEvent();
        public static event UpdateEvent OnFixedUpdate;
        public static event UpdateEvent OnUpdate;
        public static event UpdateEvent OnLateUpdate;

        
        private void FixedUpdate()
        {
            if (CanFixedUpdate)
            {
                OnFixedUpdate?.Invoke();
            }
            
            FixedUpdateCount++;
        }
        
        private void Update()
        {
            if (CanUpdate)
            {
                OnUpdate?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (CanUpdate)
            {
                OnLateUpdate?.Invoke();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (s_instance == null)
            {
                s_instance = CreateInstance();
            }
        }
    }
}