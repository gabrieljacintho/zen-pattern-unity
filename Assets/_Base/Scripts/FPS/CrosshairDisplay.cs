using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.FPS
{
    [RequireComponent(typeof(RectTransform))]
    public class CrosshairDisplay : MonoBehaviour
    {
        [SerializeField] private CrosshairData _defaultCrosshair;
        
        [Space]
        [SerializeField] private Image _leftImage;
        [SerializeField] private Image _rightImage;
        [SerializeField] private Image _centerImage;
        [SerializeField] private Image _topImage;
        [SerializeField] private Image _bottomImage;

        [Header("Settings")]
        [SerializeField] private float _pixelsPerUnit = 100f;
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _minRadius;
        [Tooltip("Set negative to not limit.")]
        [SerializeField] private float _maxRadius = -1;
        [SerializeField] private float _defaultRadius;
        [SerializeField] private float _alpha = 0.5f;

        private RectTransform _rectTransform;
        private EquipmentSwapper _equipmentSwapper;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _equipmentSwapper = GameObjectID.FindComponentInChildrenWithID<EquipmentSwapper>(GameObjectID.PlayerID, true);
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }
            
            CrosshairData data = GetCurrentCrosshairData();
            SetCrosshair(data);

            float radius = GetCurrentRadius();
            MoveTowardsRadius(radius);
        }

        private CrosshairData GetCurrentCrosshairData()
        {
            Equipment.Equipment selectedEquipment = _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;
            if (selectedEquipment != null)
            {
                return selectedEquipment.CurrentPrecision.Crosshair;
            }

            return _defaultCrosshair;
        }

        private float GetCurrentRadius()
        {
            Equipment.Equipment selectedEquipment = _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;
            if (selectedEquipment != null)
            {
                return selectedEquipment.CurrentPrecision.Radius;
            }

            return _defaultRadius;
        }
        
        private void SetCrosshair(CrosshairData data)
        {
            if (_leftImage != null)
            {
                Sprite sprite = data != null ? data.leftSprite : null;
                Rect rect = data != null ? data.leftRect : default;
                UpdateImage(_leftImage, sprite, rect);
            }

            if (_rightImage != null)
            {
                Sprite sprite = data != null ? data.rightSprite : null;
                Rect rect = data != null ? data.rightRect : default;
                UpdateImage(_rightImage, sprite, rect);
            }

            if (_centerImage != null)
            {
                Sprite sprite = data != null ? data.centerSprite : null;
                Rect rect = data != null ? data.centerRect : default;
                UpdateImage(_centerImage, sprite, rect);
            }

            if (_topImage != null)
            {
                Sprite sprite = data != null ? data.topSprite : null;
                Rect rect = data != null ? data.topRect : default;
                UpdateImage(_topImage, sprite, rect);
            }

            if (_bottomImage != null)
            {
                Sprite sprite = data != null ? data.bottomSprite : null;
                Rect rect = data != null ? data.bottomRect : default;
                UpdateImage(_bottomImage, sprite, rect);
            }
        }

        private void UpdateImage(Image image, Sprite sprite)
        {
            float targetAlpha = _alpha;
            if (sprite == null)
            {
                targetAlpha = 0f;
            }
            else
            {
                image.SetAlpha(0f);
                image.sprite = sprite;
            }

            LerpAlpha(image, targetAlpha);
        }

        private void UpdateImage(Image image, Sprite sprite, Rect rect)
        {
            UpdateImage(image, sprite);

            if (sprite != null)
            {
                image.rectTransform.SetRect(rect);
            }
        }

        private void LerpAlpha(Graphic image, float target)
        {
            float currentAlpha = image.color.a;
            currentAlpha = Mathf.MoveTowards(currentAlpha, target, _speed * _pixelsPerUnit * Time.deltaTime);
            
            image.SetAlpha(currentAlpha);
        }

        private void MoveTowardsRadius(float target = 0f)
        {
            float currentRadius = _rectTransform.rect.width / 2f;
            
            target *= _pixelsPerUnit;
            target = Mathf.Max(target, _minRadius, 0f);
            if (_maxRadius >= 0f)
            {
                target = Mathf.Min(target, _maxRadius);
            }
            
            currentRadius = Mathf.MoveTowards(currentRadius, target, _speed * _pixelsPerUnit * Time.deltaTime);
            
            _rectTransform.SetSizeWithCurrentAnchors(Vector2.one * (currentRadius * 2f));
        }
    }
}