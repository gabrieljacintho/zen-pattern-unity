using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentArms : EquipmentComponent
    {
        private TransformOffset _armsOffset;

        protected override bool CanUpdate => base.CanUpdate && ArmsOffset != null;
        protected TransformOffset ArmsOffset
        {
            get
            {
                if (_armsOffset == null)
                {
                    _armsOffset = GetComponentInParent<TransformOffset>();
                }

                return _armsOffset;
            }
        }
        protected EquipmentStateData CurrentStateData => GetCurrentStateData();


        protected virtual void LateUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }
            
            UpdateArmsOffset();
        }

        protected virtual void UpdateArmsOffset()
        {
            if (ArmsOffset == null)
            {
                return;
            }
            
            ArmsOffset.PositionOffset = GetPositionOffset();
            ArmsOffset.RotationOffset = GetRotationOffset();
            ArmsOffset.Speed = GetSpeed();
        }

        protected virtual Vector3 GetPositionOffset()
        {
            EquipmentStateData stateData = GetCurrentStateData();
            return stateData != null ? stateData.ArmsPositionOffset : Vector3.zero;
        }
        
        protected virtual Vector3 GetRotationOffset()
        {
            EquipmentStateData stateData = GetCurrentStateData();
            return stateData != null ? stateData.ArmsRotationOffset : Vector3.zero;
        }
        
        protected virtual float GetSpeed()
        {
            EquipmentStateData stateData = GetCurrentStateData();
            return stateData != null ? stateData.ArmsSpeed : 1f;
        }

        protected virtual EquipmentStateData GetCurrentStateData()
        {
            return Equipment.GetCurrentStateData();
        }
    }
}