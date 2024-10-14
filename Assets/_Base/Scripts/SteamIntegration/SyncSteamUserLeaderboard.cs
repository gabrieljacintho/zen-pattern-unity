using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Steamworks;
using Steamworks.Data;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.SteamIntegration
{
    [Serializable]
    public enum SteamLeaderboardType
    {
        Int,
        Stat
    }
    
    public class SyncSteamUserLeaderboard : MonoBehaviour
    {
        [SerializeField] private SteamLeaderboardType _type;
        [SerializeField] protected string _leaderboardId;
        [ShowIf("@_type == SteamLeaderboardType.Int")]
        [SerializeField] private IntVariable _intVariable;
        [FormerlySerializedAs("_statId")]
        [ShowIf("@_type == SteamLeaderboardType.Stat")]
        [SerializeField] private string _statName;
        [SerializeField] private bool _replaceScore;
        
        private Leaderboard? _leaderboard;

        private Task _loadLeaderboardAsyncTask;
        
        
        private void Awake()
        {
            _loadLeaderboardAsyncTask = LoadLeaderboardAsync();
        }
                
        private void OnEnable()
        {
            RegisterListeners();
            UpdateScore();
        }
        
        private void OnDisable()
        {
            UnregisterListeners();
        }
                
        public void UpdateScore()
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            switch (_type)
            {
                case SteamLeaderboardType.Int:
                    if (_intVariable != null)
                    {
                        SetScoreAsync(_intVariable.Value);
                    }
                    break;
                
                case SteamLeaderboardType.Stat:
                    if (!string.IsNullOrEmpty(_statName))
                    {
                        SetScoreAsync(SteamUserStats.GetStatInt(_statName));
                    }
                    break;
            }
        }
        
        private void UpdateScore(string statName)
        {
            if (statName == _statName)
            {
                UpdateScore();
            }
        }
        
        private async void SetScoreAsync(int value)
        {
            await _loadLeaderboardAsyncTask;
            
            if (!_leaderboard.HasValue)
            {
                return;
            }
            
            if (_replaceScore)
            {
                await _leaderboard.Value.ReplaceScore(value);
            }
            else
            {
                await _leaderboard.Value.SubmitScoreAsync(value);
            }
            
            SteamManager.StoreStats();
        }
        
        private async Task LoadLeaderboardAsync()
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            _leaderboard = await SteamUserStats.FindLeaderboardAsync(_leaderboardId);
            
            if (!_leaderboard.HasValue)
            {
                Debug.LogError("Leaderboard \"" + _leaderboardId + "\" not found!", this);
            }
        }
        
        private void RegisterListeners()
        {
            switch (_type)
            {
                case SteamLeaderboardType.Int:
                    if (_intVariable != null)
                    {
                        _intVariable.Changed.Register(UpdateScore);
                    }
                    break;
                
                case SteamLeaderboardType.Stat:
                    if (!string.IsNullOrEmpty(_statName))
                    {
                        SteamManager.StatChanged += UpdateScore;
                    }
                    break;
            }
        }

        private void UnregisterListeners()
        {
            switch (_type)
            {
                case SteamLeaderboardType.Int:
                    if (_intVariable != null)
                    {
                        _intVariable.Changed.Unregister(UpdateScore);
                    }
                    break;
                
                case SteamLeaderboardType.Stat:
                    if (!string.IsNullOrEmpty(_statName))
                    {
                        SteamManager.StatChanged -= UpdateScore;
                    }
                    break;
            }
        }
    }
}