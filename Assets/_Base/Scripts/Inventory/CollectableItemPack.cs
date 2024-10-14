using FireRingStudio.Pool;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.Inventory
{
    public class CollectableItemPack : CollectableItem
    {
        [Header("Pack")]
        [SerializeField, ES3NonSerializable] private IntReference _minSizeReference;
        [SerializeField, ES3NonSerializable] private IntReference _maxSizeReference;
        
        [ES3Serializable] private int _size = -1;
        
        public override int Quantity
        {
            get
            {
                if (_size < 0)
                {
                    _size = GetRandomSize();
                }

                return _size;
            }
        }
        private int MinQuantity => _minSizeReference?.Value ?? 0;
        private int MaxQuantity => _maxSizeReference?.Value ?? 0;


        protected override void Awake()
        {
            base.Awake();

            if (TryGetComponent(out PooledObject pooledObject))
            {
                pooledObject.OnGetAction += OnGet;
            }
        }

        protected override void OnCollectFunc()
        {
            base.OnCollectFunc();
            _size = -1;
        }
        
        private int GetRandomSize()
        {
            int value = UnityEngine.Random.Range(MinQuantity, MaxQuantity + 1);
            return Mathf.Max(value, 0);
        }

        private void OnGet()
        {
            _size = -1;
        }
    }
}
