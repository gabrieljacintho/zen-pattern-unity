using System;
using System.Collections;
using FireRingStudio.Patterns;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace FireRingStudio.SteamIntegration
{
    public class SteamManager : PersistentSingleton<SteamManager>
    {
        [SerializeField] private uint _appID;
#if UNITY_EDITOR
        [SerializeField] private bool _editor;
#endif

        private static readonly WaitForSeconds s_storeStatsDelay = new WaitForSeconds(60f);
        private static Coroutine s_storeStatsRoutine;

        public static Action<string> StatChanged;

        
        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!_editor)
            {
                Destroy(gameObject);
                return;
            }
#endif
            base.Awake();
            Initialize();
        }
    
        private void OnEnable()
        {
            SteamFriends.OnGameOverlayActivated += OnGameOverlayActivatedFunc;
            SteamUserStats.OnAchievementProgress += OnAchievementProgressFunc;
        }

        private void OnDisable()
        {
            SteamFriends.OnGameOverlayActivated -= OnGameOverlayActivatedFunc;
            SteamUserStats.OnAchievementProgress -= OnAchievementProgressFunc;
            
            if (SteamClient.IsValid)
            {
                SteamUserStats.StoreStats();
                SteamClient.Shutdown();
            }
        }

        public static void SetStat(string name, int value)
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            SteamUserStats.SetStat(name, value);
            StatChanged?.Invoke(name);
            
            StoreStats();
        }
        
        public static void SetStat(string name, float value)
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            SteamUserStats.SetStat(name, value);
            StatChanged?.Invoke(name);
            
            StoreStats();
        }
        
        public static void AddStat(string name, int value = 1)
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            SteamUserStats.AddStat(name, value);
            StatChanged?.Invoke(name);
            
            StoreStats();
        }
        
        public static void AddStat(string name, float value = 1f)
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            SteamUserStats.AddStat(name, value);
            StatChanged?.Invoke(name);
            
            StoreStats();
        }

        public static void StoreStats()
        {
            if (Instance != null)
            {
                if (s_storeStatsRoutine == null)
                {
                    s_storeStatsRoutine = Instance.StartCoroutine(StoreStatsRoutine());
                }
            }
            else if (SteamClient.IsValid)
            {
                SteamUserStats.StoreStats();
            }
        }

        public static void ResetStats(bool includeAchievements)
        {
            if (!SteamClient.IsValid)
            {
                return;
            }
            
            SteamUserStats.ResetAll(includeAchievements);
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();

            Debug.Log("Steam " + (includeAchievements ? "stats and achievements reset." : "stats reset."));
        }

        public static bool FindAchievement(string id, out Achievement achievement)
        {
            return FindAchievement(achievement => achievement.Identifier == id, out achievement);
        }

        public static bool FindAchievement(Predicate<Achievement> match, out Achievement achievement)
        {
            if (!SteamClient.IsValid)
            {
                return false;
            }
            
            foreach (Achievement other in SteamUserStats.Achievements)
            {
                if (match.Invoke(other))
                {
                    achievement = other;
                    return true;
                }
            }

            return false;
        }

        private static IEnumerator StoreStatsRoutine()
        {
            if (!SteamClient.IsValid)
            {
                yield break;
            }
            
            while (!SteamUserStats.StoreStats())
            {
                yield return s_storeStatsDelay;
            }

            s_storeStatsRoutine = null;
        }

        private void Initialize()
        {
            try
            {
#if !UNITY_EDITOR
                if (SteamClient.RestartAppIfNecessary(_appID))
                {
                    Application.Quit();
                    return;
                }
#endif                
                SteamClient.Init(_appID);
                Debug.Log("Steamworks initialized. (\"" + SteamClient.Name + "\")");
            }
            catch (Exception exception)
            {
                // Something went wrong - it's one of these:
                //
                //     Steam is closed?
                //     Can't find steam_api dll?
                //     Don't have permission to play app?
                //
                Debug.LogError("Could not initialize Steamworks!" + Environment.NewLine + exception);
                Application.Quit();
            }
        }

        private static void OnGameOverlayActivatedFunc(bool enabled)
        {
            if (enabled && GameManager.InAnyGameState)
            {
                GameManager.SetPaused(true);
            }
        }
        
        private static void OnAchievementProgressFunc(Achievement achievement, int currentProgress, int maxProgress)
        {
            if (achievement.State)
            {
                Debug.Log($"Steam achievement \"{achievement.Name}\" was unlocked.");
            }
        }
    }
}