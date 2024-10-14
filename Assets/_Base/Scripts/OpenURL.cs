using I2.Loc;
#if STEAMWORKS
using Steamworks;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    public class OpenURL : MonoBehaviour
    {
        [SerializeField] private LocalizedString _localizedUrl;
        [FormerlySerializedAs("_url")]
        [SerializeField] private string _defaultUrl;


        public void Open()
        {
            string url;
            if (!string.IsNullOrEmpty(_localizedUrl))
            {
                url = _localizedUrl;
            }
            else if (!string.IsNullOrEmpty(_defaultUrl))
            {
                url = _defaultUrl;
            }
            else
            {
                return;
            }

            Open(url);
        }

        public static void Open(string url)
        {
#if STEAMWORKS
            try
            {
                SteamFriends.OpenWebOverlay(url);
            }
            catch
            {
                Application.OpenURL(url);
            }
#else
            Application.OpenURL(url);
#endif
        }
    }
}