using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Spawn
{
    [RequireComponent(typeof(Spawner))]
    public class SpawnerAutomation : Timer
    {
        [Header("Spawn")]
        [Tooltip("Set negative to not limit.")]
        [SerializeField] protected IntReference _quantity = new(1);
        [SerializeField] protected bool _invisible;

        private Spawner _spawner;

        private int Quantity => _quantity?.Value ?? 0;


        protected override void Awake()
        {
            base.Awake();
            _spawner = GetComponent<Spawner>();
        }

        protected override void OnStartTimer() { }

        protected override void OnStopTimer() { }

        public override void OnEndTimer()
        {
            if (Quantity > 0)
            {
                _spawner.SpawnQuantity(Quantity, _invisible);
            }
            else if (Quantity < 0)
            {
                _spawner.SpawnAll(_invisible);
            }
        }

        protected override bool CanRun()
        {
            return base.CanRun() && _spawner.CanSpawn();
        }
    }
}