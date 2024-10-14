using FireRingStudio.Cache;
using FireRingStudio.FPS.Equipment;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Use Equipment Data", fileName = "New Use Equipment Data")]
    public class UseEquipmentData : EquipmentData
    {
        #region Use

        [FoldoutGroup("Use")] public EquipmentUseStateInfo UsingState;
        [Tooltip("Set negative to not limit.")]
        [FoldoutGroup("Use")] public IntReference UsesPerMinuteReference = new(60);
        [FoldoutGroup("Use"), Range(0f, 1f)] public float UseSensitivityScale = 1f;
        [FoldoutGroup("Use"), Range(0f, 1f)] public float GamepadUseSensitivityScale = 1f;

        public int UsesPerMinute => UsesPerMinuteReference?.Value ?? 0;
        
        [Header("Animations")]
        [FoldoutGroup("Use")] public bool HasUseAnimation;
        [ShowIf("HasUseAnimation")]
        [FoldoutGroup("Use")] public AnimationInfo UseAnimationInfo;
        [ShowIf("HasUseAnimation")]
        [FoldoutGroup("Use")] public string UseAnimationIndexParameterName;
        [ShowIf("HasUseAnimation")]
        [FoldoutGroup("Use"), Min(1)] public int QuantityOfUseAnimations = 1;

        public int UseAnimationIndexParameterId => !string.IsNullOrEmpty(UseAnimationIndexParameterName)
            ? AnimationParameterIds.GetId(UseAnimationIndexParameterName) : -1;
        
        [Header("SFXs")]
        [FoldoutGroup("Use")] public EventReference[] PreparingSFXs;
        [FoldoutGroup("Use")] public EventReference[] UseSFXs;
        [FoldoutGroup("Use")] public float UseNoiseRadius;

        #endregion

        #region Camera

        [FoldoutGroup("Camera")] public bool SnapCameraFOVWhenUse;

        #endregion

        #region User Interface

        [Header("Use")]
        [FoldoutGroup("User Interface")] public string UseUserInterfaceID;
        [FoldoutGroup("User Interface")] public float UseUserInterfaceDelay;

        #endregion
    }
}