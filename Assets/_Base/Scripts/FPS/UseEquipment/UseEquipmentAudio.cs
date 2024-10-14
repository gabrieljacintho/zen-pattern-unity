using FireRingStudio.Extensions;
using FireRingStudio.FPS.Equipment;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentAudio : EquipmentAudio
    {
        private UseEquipment _useEquipment;
        private UseEquipmentAnimation _useEquipmentAnimation;
        
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
        private int LastUseAnimationIndex => _useEquipmentAnimation != null ? _useEquipmentAnimation.LastUseAnimationIndex : 0;
        
        
        protected override void Awake()
        {
            base.Awake();

            _useEquipmentAnimation = GetComponentInChildren<UseEquipmentAnimation>(true);
            if (_useEquipmentAnimation != null)
            {
                _useEquipmentAnimation.UseAnimationPlayed += PlayPreparingSFX;
            }
            else
            {
                _useEquipment.OnStartUsingEvent.AddListener(PlayPreparingSFX);
            }
            
            UseEquipment.OnUseEvent.AddListener(PlayUseSFX);
        }

        protected StudioEventEmitter PlaySFX(EventReference sfx, float noiseRadius = 0f)
        {
            return sfx.Play(transform.position, false, true, noiseRadius);
        }
        
        protected StudioEventEmitter PlaySFX(EventReference[] sfxs, float noiseRadius = 0f)
        {
            if (sfxs.Length == 0)
            {
                return null;
            }
            
            int index = Mathf.Clamp(LastUseAnimationIndex, 0, sfxs.Length - 1);
            
            return !sfxs[index].IsNull ? PlaySFX(sfxs[index], noiseRadius) : null;
        }
        
        private void PlayPreparingSFX()
        {
            if (Data != null && Data.PreparingSFXs != null)
            {
                PlaySFX(Data.PreparingSFXs);
            }
        }
        
        private void PlayUseSFX()
        {
            if (Data != null && Data.UseSFXs != null)
            {
                PlaySFX(Data.UseSFXs, Data.UseNoiseRadius);
            }
        }
    }
}