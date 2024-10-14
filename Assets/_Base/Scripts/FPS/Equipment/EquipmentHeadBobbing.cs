using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentHeadBobbing : EquipmentCameraComponent<HeadBobbing>
    {
        protected override void LateUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }

            base.LateUpdate();

            EquipmentStateData equipmentStateData = Equipment != null ? Equipment.GetCurrentStateData() : null;
            if (equipmentStateData != null && equipmentStateData.OverrideHeadBobbingSettings)
            {
                if (_worldCameraComponent != null)
                {
                    _worldCameraComponent.OverrideSettings(equipmentStateData.WorldBobbingSettings);
                }

                if (_firstPersonCameraComponent != null)
                {
                    _firstPersonCameraComponent.OverrideSettings(equipmentStateData.FirstPersonBobbingSettings);
                }
            }
            else
            {
                if (_worldCameraComponent != null)
                {
                    _worldCameraComponent.DisableOverrideSettings();
                }

                if (_firstPersonCameraComponent != null)
                {
                    _firstPersonCameraComponent.DisableOverrideSettings();
                }
            }
        }

        protected override void UpdateCameraComponent(HeadBobbing cameraComponent)
        {
            cameraComponent.AmplitudeScale.SetComponentValue(this, GetHeadBobbingAmplitudeScale());
            cameraComponent.FrequencyScale.SetComponentValue(this, GetHeadBobbingFrequencyScale());
        }

        protected override void ResetCameraComponent(HeadBobbing cameraComponent)
        {
            cameraComponent.DisableOverrideSettings();
            cameraComponent.AmplitudeScale.RemoveComponent(this);
            cameraComponent.FrequencyScale.RemoveComponent(this);
        }

        protected virtual float GetHeadBobbingAmplitudeScale()
        {
            EquipmentStateData equipmentStateData = Equipment != null ? Equipment.GetCurrentStateData() : null;
            return equipmentStateData != null ? equipmentStateData.HeadBobbingAmplitudeScale : 1f;
        }
        
        protected virtual float GetHeadBobbingFrequencyScale()
        {
            EquipmentStateData equipmentStateData = Equipment != null ? Equipment.GetCurrentStateData() : null;
            return equipmentStateData != null ? equipmentStateData.HeadBobbingFrequencyScale : 1f;
        }
    }
}