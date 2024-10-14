using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentCameraFOV : EquipmentCameraComponent<CameraFOVHandler>
    {
        protected override void UpdateCameraComponent(CameraFOVHandler cameraComponent)
        {
            float fov = cameraComponent == _worldCameraComponent ? GetWorldCameraFOV() : GetFirstPersonCameraFOV();
            
            if (fov > 0f)
            {
                cameraComponent.Target = fov;
            }
            else
            {
                cameraComponent.ResetToDefault();
            }
        }

        protected override void ResetCameraComponent(CameraFOVHandler cameraComponent)
        {
            cameraComponent.ResetToDefault();
        }

        protected virtual float GetWorldCameraFOV()
        {
            if (Data == null)
            {
                return 0f;
            }
            
            float value = Data.WorldCameraFOV;
            
            EquipmentStateData stateData = Equipment != null ? Equipment.GetCurrentStateData() : null;
            if (stateData != null)
            {
                value *= stateData.WorldCameraFOVScale;
            }
            
            return value;
        }

        protected virtual float GetFirstPersonCameraFOV()
        {
            if (Data == null)
            {
                return 0f;
            }
            
            float value = Data.FirstPersonCameraFOV;
            
            EquipmentStateData stateData = Equipment != null ? Equipment.GetCurrentStateData() : null;
            if (stateData != null)
            {
                value *= stateData.FirstPersonCameraFOVScale;
            }
            
            return value;
        }
    }
}