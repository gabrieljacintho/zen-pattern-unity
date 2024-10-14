using Steamworks.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.SteamIntegration
{
    public class SteamUserAchievement : MonoBehaviour
    {
        [FormerlySerializedAs("_achievementId")]
        [FormerlySerializedAs("_achievementName")]
        [SerializeField] private string _id;
        [SerializeField] private bool _achieveOnStart;

        
        private void Start()
        {
            if (_achieveOnStart)
            {
                Achieve();
            }
        }

        public void Achieve()
        {
            Achieve(_id);
        }
        
        public void Achieve(string achievementId)
        {
            if (!SteamManager.FindAchievement(achievementId, out Achievement achievement))
            {
                Debug.LogError("Achievement \"" + achievementId + "\" not found!", this);
                return;
            }

            achievement.Trigger();
            SteamManager.StoreStats();
        }

        public void ResetAchievement()
        {
            ResetAchievement(_id);
        }

        public void ResetAchievement(string achievementId)
        {
            if (!SteamManager.FindAchievement(achievementId, out Achievement achievement))
            {
                Debug.LogError("Achievement \"" + achievementId + "\" not found!", this);
                return;
            }

            achievement.Clear();
            SteamManager.StoreStats();
        }
    }
}