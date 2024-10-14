using FireRingStudio.Inventory;
using FireRingStudio.Movement;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [CreateAssetMenu(menuName = "FireRing Studio/FPS/Equipment Data", fileName = "New Equipment Data")]
    public class EquipmentData : ItemData
    {
        #region Equip
        
        [FoldoutGroup("Equip")] public bool CanAutoEquip = true;
        [FoldoutGroup("Equip")] public MovementStateValues<EquipmentStateData> DefaultStateDataset;
        [FoldoutGroup("Equip")] public MovementStateValues<Precision> DefaultPrecisions;
        
        [Header("Animations")]
        [FoldoutGroup("Equip")] public bool HasEquipAnimation = true;
        [ShowIf("HasEquipAnimation")]
        [FoldoutGroup("Equip")] public AnimationInfo EquipAnimationInfo;
        [FoldoutGroup("Equip")] public bool HasUnequipAnimation = true;
        [ShowIf("HasUnequipAnimation")]
        [FoldoutGroup("Equip")] public AnimationInfo UnequipAnimationInfo;
        
        [Header("SFXs")]
        [FoldoutGroup("Equip")] public EventReference EquipSFX;
        [FoldoutGroup("Equip")] public EventReference UnequipSFX;

        #endregion

        #region Movement

        [FoldoutGroup("Movement")] public FloatReference MoveSpeedScaleReference = new(1f);
        [FoldoutGroup("Movement")] public bool CanSprint = true;

        public float MoveSpeedScale => MoveSpeedScaleReference?.Value ?? 1f;

        #endregion

        #region Camera

        [FoldoutGroup("Camera")] public FloatReference WorldCameraFOVReference = new(60f);
        [FoldoutGroup("Camera")] public FloatReference FirstPersonCameraFOVReference = new(45f);
        [FoldoutGroup("Camera")] public FloatReference LookSensitivityScaleReference = new(1f);

        public float WorldCameraFOV => WorldCameraFOVReference?.Value ?? 0f;
        public float FirstPersonCameraFOV => FirstPersonCameraFOVReference?.Value ?? 0f;
        public float LookSensitivityScale => LookSensitivityScaleReference?.Value ?? 1f;

        #endregion

        #region User Interface

        [FoldoutGroup("User Interface")] public string UserInterfaceID;
        [FoldoutGroup("User Interface")] public float UserInterfaceDelay;

        #endregion
    }
}
