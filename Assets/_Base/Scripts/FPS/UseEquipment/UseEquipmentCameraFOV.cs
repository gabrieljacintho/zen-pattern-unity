using FireRingStudio.FPS.Equipment;
using FireRingStudio.FPS.ProjectileWeapon;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentCameraFOV : EquipmentCameraFOV
    {
        private UseEquipment _useEquipment;

        private bool _wasUsing;

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


        protected override void UpdateCameraComponent(CameraFOVHandler cameraComponent)
        {
            if (Equipment.InUse && Data != null && Data.SnapCameraFOVWhenUse)
            {
                if (UseEquipment.CanShowUseUserInterface)
                {
                    base.UpdateCameraComponent(cameraComponent);
                    cameraComponent.Snap();
                    _wasUsing = true;
                }
            }
            else
            {
                base.UpdateCameraComponent(cameraComponent);

                if (_wasUsing)
                {
                    cameraComponent.Snap();
                    _wasUsing = false;
                }
            }
        }
    }
}