using System.Collections.Generic;
using FireRingStudio.Patterns;
using FireRingStudio.Pool;
using FireRingStudio.Spawn;
using FireRingStudio.Vitals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Wave
{
    public class WaveManager : Singleton<WaveManager>
    {
        [SerializeField] private Spawner[] _enemySpawners;

        [Space]
        public UnityEvent<int> onWaveChanged;

        private readonly Dictionary<PooledObject, Health> _enemyHealths = new Dictionary<PooledObject, Health>();

        public delegate void WaveEvent(int wave);
        public static event WaveEvent WaveChanged;

        public static int CurrentWave
        {
            get => s_currentWave;
            private set => SetCurrentWave(value);
        }

        private static int s_currentWave;


        private void OnEnable()
        {
            WaveChanged += onWaveChanged.Invoke;
        }

        private void OnDisable()
        {
            WaveChanged -= onWaveChanged.Invoke;
        }

        [Button]
        public void StartWaves()
        {
            CurrentWave = 0;
        }

        [Button]
        public void NextWave()
        {
            CurrentWave++;
        }
        
        [Button]
        public void UpdateWave()
        {
            if (_enemySpawners == null || _enemySpawners.Length == 0)
            {
                return;
            }
            
            foreach (Spawner spawner in _enemySpawners)
            {
                if (spawner.CanSpawn())
                {
                    return;
                }
                
                foreach (PooledObject instance in spawner.Instances)
                {
                    if (TryGetHealth(instance, out Health health))
                    {
                        if (!health.IsDead)
                        {
                            return;
                        }
                    }
                }
            }
            
            NextWave();
        }

        [Button]
        public static void SetCurrentWave(int value)
        {
            if (s_currentWave == value)
                return;
            
            s_currentWave = value;
            WaveChanged?.Invoke(s_currentWave);
        }

        private bool TryGetHealth(PooledObject pooledObject, out Health health)
        {
            if (_enemyHealths.TryGetValue(pooledObject, out health))
            {
                return health != null;
            }
            
            health = pooledObject.GetComponent<Health>();
            _enemyHealths.Add(pooledObject, health);

            return health != null;
        }
    }
}