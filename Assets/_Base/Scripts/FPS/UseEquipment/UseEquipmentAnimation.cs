using FireRingStudio.FPS.Equipment;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentAnimation : EquipmentAnimation
    {
        private UseEquipment _useEquipment;
        
        private bool _initialized;
        private int _lastUseAnimationIndex;

        public int LastUseAnimationIndex => _lastUseAnimationIndex;
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
        
        public delegate void AnimationEvent();
        public event AnimationEvent UseAnimationPlayed;
        
        
        protected override void Initialize()
        {
            base.Initialize();
            
            if (Data != null && Data.HasUseAnimation)
            {
                UpdateAnimationSpeed(Data.UseAnimationInfo);
            }

            if (!_initialized)
            {
                OnInitializeFirst();
                _initialized = true;
            }
        }

        protected virtual void OnInitializeFirst()
        {
            if (Equipment == null)
            {
                return;
            }
            
            UseEquipment.OnStartUsingEvent.AddListener(PlayUseAnimation);
            UseEquipment.HoldAction += HoldAnimation;
            UseEquipment.ReleaseAction += ReleaseAnimation;
        }
        
        private void PlayUseAnimation()
        {
            if (Data == null)
            {
                return;
            }
            
            RandomAnimationIndex();

            if (Data.HasUseAnimation)
            {
                EquipmentAnimator.SetTrigger(Data.UseAnimationInfo.TriggerId);
            }
            
            UseAnimationPlayed?.Invoke();
        }
        
        private void RandomAnimationIndex()
        {
            if (Data == null)
            {
                return;
            }
            
            _lastUseAnimationIndex = UnityEngine.Random.Range(0, Data.QuantityOfUseAnimations);
            
            if (Data.UseAnimationIndexParameterId != -1)
            {
                EquipmentAnimator.SetFloat(Data.UseAnimationIndexParameterId, _lastUseAnimationIndex);
            }
        }

        private void HoldAnimation()
        {
            if (Data != null)
            {
                SetAnimationSpeed(Data.UseAnimationInfo, 0f);
            }
        }
        
        private void ReleaseAnimation()
        {
            if (Data != null)
            {
                UpdateAnimationSpeed(Data.UseAnimationInfo);
            }
        }
    }
}