using System;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Energy
{
    public class LowBatteryEvent : BatteryComponent
    {
        [SerializeField, Range(0f, 1f)] private float _lowBatteryThreshold = 0.3f;
        [SerializeField] private bool _onlyWhileDischarging;

        private bool _lowBattery;
        
        [Space]
        public UnityEvent OnLowBattery;
        public UnityEvent OnNotLowBattery;

        
        private void OnEnable()
        {
            InvokeEvent();
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused || _lowBattery == IsLowBattery())
            {
                return;
            }

            InvokeEvent();
        }

        public void InvokeEvent()
        {
            _lowBattery = IsLowBattery();
            
            if (_lowBattery)
            {
                OnLowBattery?.Invoke();
            }
            else
            {
                OnNotLowBattery?.Invoke();
            }
        }

        private bool IsLowBattery()
        {
            if (Battery == null)
            {
                return false;
            }
            
            if (_onlyWhileDischarging && !Battery.Discharging.Result)
            {
                return false;
            }
            
            return Battery.BatteryLevel <= _lowBatteryThreshold;
        }
    }
}