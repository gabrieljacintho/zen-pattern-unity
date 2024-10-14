using TMPro;
using UnityEngine;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionText : MonoBehaviour
    {
        public TextMeshProUGUI Text { get; private set; }

        public string prefix;
        public string suffix = "v";


        private void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
            if (Text != null)
                Text.text = prefix + Application.version + suffix;
            else
                Debug.LogNo(nameof(TextMeshProUGUI));
        }
    }
}