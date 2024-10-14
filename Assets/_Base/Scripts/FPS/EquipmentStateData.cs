using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Equipment State Data", fileName = "New Equipment State Data")]
    public class EquipmentStateData : ScriptableObject
    {
        [Header("Arms")]
        public Vector3 ArmsPositionOffset;
        public Vector3 ArmsRotationOffset;
        public float ArmsSpeed = 1f;

        [Header("Camera FOV")]
        [Range(0f, 2f)] public float WorldCameraFOVScale = 1f;
        [Range(0f, 2f)] public float FirstPersonCameraFOVScale = 1f;

        [Header("Head Bobbing")]
        public bool OverrideHeadBobbingSettings;
        [ShowIf("OverrideHeadBobbingSettings")] public Movement.HeadBobbingSettings WorldBobbingSettings;
        [ShowIf("OverrideHeadBobbingSettings")] public Movement.HeadBobbingSettings FirstPersonBobbingSettings;
        [Range(-2f, 4f)] public float HeadBobbingAmplitudeScale = 1f;
        [Range(-2f, 4f)] public float HeadBobbingFrequencyScale = 1f;
    }
}