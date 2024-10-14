using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Vitals
{
    [RequireComponent(typeof(Image))]
    public class DamageImage : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _maxAlpha = 1f;
        [SerializeField] private float _smoothTime = 0.3f;

        private Health _health;
        private Image _image;
        private float _velocity;


        private void Awake()
        {
            _image = GetComponent<Image>();

            this.DoOnNextFrame(() =>
            {
                _health = GameObjectID.FindComponentInGameObjectWithID<Health>(GameObjectID.PlayerID, true);
                if (_health != null)
                {
                    _health.onRevive.AddListener(OnRevive);
                }
            });
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.onRevive.RemoveListener(OnRevive);
            }
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            float alpha = GetTargetAlpha();
            alpha = Mathf.SmoothDamp(_image.color.a, alpha, ref _velocity, _smoothTime);

            SetAlpha(alpha);
        }

        private float GetTargetAlpha()
        {
            if (_health == null)
            {
                return 0f;
            }

            float target = Mathf.Lerp(0f, 1f, _health.CurrentHealth / _health.MaxHealth);
            target -= 1f;

            return Mathf.Abs(target) * _maxAlpha;
        }

        private void SetAlpha(float value)
        {
            Color color = _image.color;
            color.a = value;
            _image.color = color;
        }

        private void OnRevive()
        {
            float alpha = GetTargetAlpha();
            SetAlpha(alpha);
        }
    }
}