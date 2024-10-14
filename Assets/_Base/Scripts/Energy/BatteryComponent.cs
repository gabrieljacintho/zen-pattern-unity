using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.Energy
{
    public abstract class BatteryComponent : MonoBehaviour
    {
        [SerializeField] private Battery _battery;
        [HideIf("@_battery != null")]
        [SerializeField] private string _batteryId;
        
        protected Battery Battery
        {
            get
            {
                if (_battery == null)
                {
                    _battery = ComponentID.FindComponentWithID<Battery>(_batteryId, true);
                }

                return _battery;
            }
        }
    }
}