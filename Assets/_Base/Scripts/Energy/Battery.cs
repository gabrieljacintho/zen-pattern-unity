using FireRingStudio.Operations;
using FireRingStudio.Save;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Energy
{
    public class Battery : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _initialBattery = 1f;
        [SerializeField] private float _dischargeSpeed = 1f;
        [SerializeField] private float _chargeSpeed = 1f;
        [SerializeField] private BoolOrOperation _charging = new(false);
        [SerializeField] private BoolOrOperation _discharging = new(false);

        [ES3Serializable] private float _batteryLevel;

        public BoolOrOperation Charging => _charging;
        public BoolOrOperation Discharging => _discharging;

        public float BatteryLevel
        {
            get => _batteryLevel;
            private set => SetBatteryLevel(value);
        }

        [Space]
        public UnityEvent<float> OnBatteryLevelChanged;


        private void Awake()
        {
            ResetToDefault();
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            if (_charging != null && _charging.Result)
            {
                BatteryLevel += _chargeSpeed * Time.deltaTime;
            }

            if (_discharging != null && _discharging.Result)
            {
                BatteryLevel -= _dischargeSpeed * Time.deltaTime;
            }
        }

        public void FullCharge()
        {
            BatteryLevel = 1f;
        }

        public void ResetToDefault()
        {
            BatteryLevel = _initialBattery;
        }

        private void SetBatteryLevel(float value)
        {
            _batteryLevel = Mathf.Clamp(value, 0f, 1f);
            OnBatteryLevelChanged?.Invoke(_batteryLevel);
        }
    }
}