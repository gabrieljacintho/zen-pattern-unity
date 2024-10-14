using FireRingStudio.Energy;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace FireRingStudio.PostProcess
{
    [RequireComponent(typeof(PostProcessVolume), typeof(Battery))]
    public class PostProcessBattery : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _weightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        private PostProcessVolume _volume;
        private Battery _battery;

        
        private void Awake()
        {
            _volume = GetComponent<PostProcessVolume>();
            _battery = GetComponent<Battery>();
        }

        private void LateUpdate()
        {
            _battery.Charging.SetComponentValue(this, !_volume.enabled);
            _battery.Discharging.SetComponentValue(this, _volume.enabled);

            float weight = _weightCurve.Evaluate(_battery.BatteryLevel);
            _volume.weight = Mathf.Clamp(weight, 0f, 1f);
        }

        private void OnDisable()
        {
            _battery.Charging.RemoveComponent(this);
            _battery.Discharging.RemoveComponent(this);
        }
    }
}