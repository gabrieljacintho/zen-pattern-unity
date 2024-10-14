using System;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.SteamIntegration
{
    [Serializable]
    public enum SteamStatType
    {
        Int,
        Float
    }
    
    public class SyncSteamUserStat : MonoBehaviour
    {
        [SerializeField] private SteamStatType _type;
        [SerializeField] protected string _statName;
        [ShowIf("@_type == SteamStatType.Int")]
        [SerializeField] private IntVariable _intVariable;
        [ShowIf("@_type == SteamStatType.Float")]
        [SerializeField] private FloatVariable _floatVariable;


        private void OnEnable()
        {
            RegisterListeners();
            UpdateStat();
        }

        private void OnDisable()
        {
            UnregisterListeners();
        }

        public void UpdateStat()
        {
            switch (_type)
            {
                case SteamStatType.Int:
                    if (_intVariable != null)
                    {
                        SteamManager.SetStat(_statName, _intVariable.Value);
                    }
                    break;
                
                case SteamStatType.Float:
                    if (_floatVariable != null)
                    {
                        SteamManager.SetStat(_statName, _floatVariable.Value);
                    }
                    break;
            }
        }

        private void RegisterListeners()
        {
            switch (_type)
            {
                case SteamStatType.Int:
                    if (_intVariable != null)
                    {
                        _intVariable.Changed.Register(UpdateStat);
                    }
                    break;
                
                case SteamStatType.Float:
                    if (_floatVariable != null)
                    {
                        _floatVariable.Changed.Register(UpdateStat);
                    }
                    break;
            }
        }
        
        private void UnregisterListeners()
        {
            switch (_type)
            {
                case SteamStatType.Int:
                    if (_intVariable != null)
                    {
                        _intVariable.Changed.Unregister(UpdateStat);
                    }
                    break;
                
                case SteamStatType.Float:
                    if (_floatVariable != null)
                    {
                        _floatVariable.Changed.Unregister(UpdateStat);
                    }
                    break;
            }
        }
    }
}