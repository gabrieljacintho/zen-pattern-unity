using TMPro;
using UnityEngine;

namespace FireRingStudio.LevelManagement
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LevelText : MonoBehaviour
    {
        private TextMeshProUGUI _text;


        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!LevelManager.HasInstance)
            {
                return;
            }

            int level = LevelManager.Instance.GetActiveLevelIndex();

            _text.text = (++level).ToString();
        }
    }
}