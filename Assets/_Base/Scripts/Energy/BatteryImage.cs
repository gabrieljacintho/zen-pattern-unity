using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Energy
{
    [RequireComponent(typeof(Image))]
    public class BatteryImage : BatteryComponent
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private Color _dischargedColor = Color.red;
        [SerializeField] private AnimationCurve _dischargedColorCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        
        private Image _image;
        private Color _defaultColor;


        private void Awake()
        {
            _image = GetComponent<Image>();
            _defaultColor = _image.color;
        }

        private void LateUpdate()
        {
            if (Battery == null)
            {
                return;
            }

            UpdateSprite(Battery.BatteryLevel);
            UpdateColor(Battery.BatteryLevel);
        }

        private void UpdateSprite(float batteryLevel)
        {
            if (_sprites == null)
            {
                return;
            }

            if (_sprites.Count == 1)
            {
                _image.sprite = _sprites[0];
                return;
            }
            
            float batteryPerSprite = 1f / (_sprites.Count - 1);
            int spriteIndex = Mathf.CeilToInt(batteryLevel / batteryPerSprite);
            _image.sprite = _sprites[spriteIndex];
        }

        private void UpdateColor(float batteryLevel)
        {
            float t = _dischargedColorCurve.Evaluate(batteryLevel);
            _image.color = Color.Lerp(_defaultColor, _dischargedColor, t);
        }
    }
}