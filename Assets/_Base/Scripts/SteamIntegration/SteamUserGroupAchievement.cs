using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace FireRingStudio.SteamIntegration
{
    public class SteamUserGroupAchievement : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private List<string> _otherIds;

        
        private void OnEnable()
        {
            SteamUserStats.OnAchievementProgress += OnAchievementProgressFunc;
            UpdateAchievement();
        }

        private void OnDisable()
        {
            SteamUserStats.OnAchievementProgress -= OnAchievementProgressFunc;
        }

        private void UpdateAchievement()
        {
            if (_otherIds == null || !SteamClient.IsValid)
            {
                return;
            }

            foreach (string otherId in _otherIds)
            {
                if (!SteamManager.FindAchievement(otherId, out Achievement otherAchievement))
                {
                    Debug.LogError("Achievement \"" + otherId + "\" not found!", this);
                    return;
                }

                if (!otherAchievement.State)
                {
                    return;
                }
            }
            
            if (!SteamManager.FindAchievement(_id, out Achievement achievement))
            {
                Debug.LogError("Achievement \"" + _id + "\" not found!", this);
                return;
            }

            achievement.Trigger();
            SteamManager.StoreStats();
        }
        
        private void OnAchievementProgressFunc(Achievement achievement, int currentProgress, int maxProgress)
        {
            if (achievement.State)
            {
                UpdateAchievement();
            }
        }
    }
}