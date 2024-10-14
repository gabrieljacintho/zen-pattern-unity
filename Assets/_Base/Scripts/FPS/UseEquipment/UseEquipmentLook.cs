using FireRingStudio.FPS.Equipment;
using FireRingStudio.Input;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentLook : EquipmentLook
    {
        private UseEquipment _useEquipment;

        protected override Equipment.Equipment Equipment => UseEquipment;
        private UseEquipment UseEquipment
        {
            get
            {
                if (_useEquipment == null)
                {
                    _useEquipment = GetComponent<UseEquipment>();
                }

                return _useEquipment;
            }
        }
        private new UseEquipmentData Data => UseEquipment.Data;


        protected override float GetLookSensitivityScale()
        {
            float value = base.GetLookSensitivityScale();

            if (Data == null)
            {
                return value;
            }

            if (UseEquipment.InUse)
            {
                if (InputManager.CurrentControlScheme == ControlScheme.KeyboardMouse)
                {
                    value *= Data.UseSensitivityScale;
                }
                else
                {
                    value *= Data.GamepadUseSensitivityScale;
                }
            }

            return value;
        }
    }
}