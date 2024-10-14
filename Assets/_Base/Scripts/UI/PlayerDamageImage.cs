using FireRingStudio.Extensions;
using FireRingStudio.Vitals;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Image))]
    public class PlayerDamageImage : MonoBehaviour
    {
        public Image Image { get; private set; }

        [SerializeField] private DamageType _damageType;
        [SerializeField, Min(0f)] private float _maxDamageAmount = 100f;
        [SerializeField, Min(0f)] private float _maxDamageDuration = 30f;

        [Header("Settings")]
        [SerializeField, Range(0f, 1f)] public float _maxAlpha = 1f;
        [SerializeField, Min(0f)] private float _smoothTime = 0.3f;
        [SerializeField] private bool _invert;
        [SerializeField] private bool _restoreOnEnable;

        private Health _playerHealth;
        
        private float _velocity;


        private void Awake()
        {
            Image = GetComponent<Image>();
            _playerHealth = GameObjectID.FindComponentInGameObjectWithID<Health>(GameObjectID.PlayerID, true);
        }

        private void OnEnable()
        {
            if (_restoreOnEnable)
            {
                Restore();
            }
        }

        private void LateUpdate()
        {
            float target = 0f;
            if (_playerHealth != null && _playerHealth.TryGetExtendedDamage(_damageType, out Damage damage))
            {
                target = Mathf.Lerp(0f, 1f, damage.Duration / _maxDamageDuration);
                target *= Mathf.Lerp(0f, 1f, damage.Amount / _maxDamageAmount);
            }
            
            if (_invert)
                target -= 1f;
                
            target = Mathf.Abs(target) * _maxAlpha;
            float alpha = Mathf.SmoothDamp(Image.color.a, target, ref _velocity, _smoothTime);

            Image.SetAlpha(alpha);
        }

        public void Restore()
        {
            Image.SetAlpha(0f);
        }
    }
}