using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio
{
    public class QualityObject : MonoBehaviour
    {
        [SerializeField] private List<KeyValue<int, GameObject>> _gameObjectByQualityLevel;


        private void OnEnable()
        {
            QualitySettings.activeQualityLevelChanged += OnActiveQualityLevelChanged;
            OnActiveQualityLevelChanged(0, QualitySettings.GetQualityLevel());
        }

        private void OnDisable()
        {
            QualitySettings.activeQualityLevelChanged -= OnActiveQualityLevelChanged;
        }

        private void OnActiveQualityLevelChanged(int previousQuality, int currentQuality)
        {
            if (_gameObjectByQualityLevel == null)
            {
                return;
            }

            foreach (KeyValue<int, GameObject> keyValue in _gameObjectByQualityLevel)
            {
                if (keyValue.Value == null)
                {
                    continue;
                }

                keyValue.Value.SetActive(keyValue.Key == currentQuality);
            }
        }
    }
}