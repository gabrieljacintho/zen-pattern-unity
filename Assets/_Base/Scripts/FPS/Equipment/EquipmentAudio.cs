using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentAudio : EquipmentComponent
    {
        protected override void Awake()
        {
            base.Awake();
            Equipment.PlayEquipFXs += PlayEquipSFX;
            Equipment.PlayUnequipFXs += PlayUnequipSFX;
        }
        
        private void PlayEquipSFX()
        {
            if (Data != null && !Data.EquipSFX.IsNull)
            {
                Data.EquipSFX.Play();
            }
        }

        private void PlayUnequipSFX()
        {
            if (Data != null && !Data.UnequipSFX.IsNull)
            {
                Data.UnequipSFX.Play();
            }
        }
    }
}