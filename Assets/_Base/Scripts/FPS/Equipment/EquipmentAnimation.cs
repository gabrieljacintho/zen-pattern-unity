using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [RequireComponent(typeof(EquipmentAnimator))]
    public class EquipmentAnimation : EquipmentComponent
    {
        private EquipmentAnimator _equipmentAnimator;

        protected EquipmentAnimator EquipmentAnimator
        {
            get
            {
                if (_equipmentAnimator == null)
                {
                    _equipmentAnimator = GetComponent<EquipmentAnimator>();
                }

                return _equipmentAnimator;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            Equipment.PlayEquipFXs += PlayEquipAnimation;
            Equipment.PlayUnequipFXs += PlayUnequipAnimation;
        }

        protected override void OnEquip()
        {
            base.OnEquip();
            Initialize();
        }

        protected virtual void Initialize()
        {
            EquipmentAnimator.Initialize();

            if (Data == null)
            {
                return;
            }
            
            if (Data.HasEquipAnimation)
            {
                UpdateAnimationSpeed(Data.EquipAnimationInfo);
            }

            if (Data.HasUnequipAnimation)
            {
                UpdateAnimationSpeed(Data.UnequipAnimationInfo);
            }
        }

        protected void SetAnimationSpeed(AnimationInfo animationInfo, float value)
        {
            int parameterId = animationInfo.SpeedParameterId;
            if (parameterId != -1)
            {
                EquipmentAnimator.SetFloat(parameterId, value);
            }
        }
        
        protected void UpdateAnimationSpeed(AnimationInfo animationInfo)
        {
            SetAnimationSpeed(animationInfo, animationInfo.Speed);
        }
        
        protected void SetTrigger(AnimationInfo animationInfo)
        {
            int parameterId = animationInfo.TriggerId;
            if (parameterId != -1)
            {
                EquipmentAnimator.SetTrigger(parameterId);
            }
        }
        
        protected void ResetTrigger(AnimationInfo animationInfo)
        {
            int parameterId = animationInfo.TriggerId;
            if (parameterId != -1)
            {
                EquipmentAnimator.ResetTrigger(parameterId);
            }
        }
        
        private void PlayEquipAnimation()
        {
            if (Data != null && Data.HasEquipAnimation)
            {
                EquipmentAnimator.SetTrigger(Data.EquipAnimationInfo.TriggerId);
            }
        }

        private void PlayUnequipAnimation()
        {
            if (Data != null && Data.HasUnequipAnimation)
            {
                EquipmentAnimator.SetTrigger(Data.UnequipAnimationInfo.TriggerId);
            }
        }
    }
}