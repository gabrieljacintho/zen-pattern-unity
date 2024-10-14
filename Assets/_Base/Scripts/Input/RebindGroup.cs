using System.Collections.Generic;
using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Input
{
    public class RebindGroup : MonoBehaviour
    {
        [SerializeField] private List<Rebind> _rebinds;
        [SerializeField] private bool _getInChildren;


        private void Awake()
        {
            if (_getInChildren)
            {
                GetInChildren();
            }
            
            Initialize();
        }

        [Button]
        public void StartRebinds()
        {
            _rebinds?.ForEach(rebind => rebind.StartRebind());
        }

        [Button]
        public void CancelRebinds()
        {
            _rebinds?.ForEach(rebind => rebind.CancelRebind());
        }

        private void Initialize()
        {
            _rebinds?.ForEach(rebind => rebind.OnRebindCompleted.AddListener(OnRebindCompleted));
        }

        private void OnRebindCompleted()
        {
            _rebinds?.ForEach(rebind => rebind.CancelRebind());
        }

        private void GetInChildren()
        {
            Rebind[] rebinds = GetComponentsInChildren<Rebind>();

            if (_rebinds == null || _rebinds.Count == 0)
            {
                _rebinds = rebinds.ToList();
                return;
            }

            foreach (Rebind rebind in rebinds)
            {
                if (!_rebinds.Contains(rebind))
                {
                    _rebinds.Add(rebind);
                }
            }
        }
    }
}