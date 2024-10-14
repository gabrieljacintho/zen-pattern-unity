using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Vitals
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private string _healthId;
        [SerializeField] private float _smoothTime = 0.3f;

        private Image _image;

        private float _velocity;


        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (_health == null && !string.IsNullOrEmpty(_healthId))
            {
                _health = ComponentID.FindComponentWithID<Health>(_healthId, true);
            }
        }

        private void LateUpdate()
        {
            if (_health == null)
            {
                return;
            }

            float targetFillAmount = Mathf.Lerp(0f, 1f, _health.CurrentHealth / _health.MaxHealth);

            if (_smoothTime > 0f)
            {
                float currentFillAmount = _image.fillAmount;
                targetFillAmount = Mathf.SmoothDamp(currentFillAmount, targetFillAmount, ref _velocity, _smoothTime);
            }

            _image.fillAmount = targetFillAmount;
        }
    }
}
