using UnityEngine;

namespace FireRingStudio.SteamIntegration
{
    public class SteamUserStat : MonoBehaviour
    {
        [SerializeField] private string _statName;

        
        public void SetStat(int value)
        {
            SteamManager.SetStat(_statName, value);
        }
        
        public void SetStat(float value)
        {
            SteamManager.SetStat(_statName, value);
        }
        
        public void AddStat(int value = 1)
        {
            SteamManager.AddStat(_statName, value);
        }
        
        public void AddStat(float value = 1f)
        {
            SteamManager.AddStat(_statName, value);
        }

        public static void ResetStats(bool includeAchievements)
        {
            SteamManager.ResetStats(includeAchievements);
        }
    }
}