using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [DisallowMultipleComponent]
    public class EquipmentLook : EquipmentComponent
    {
        private PlayerLook _playerLook;

        protected override bool CanUpdate => base.CanUpdate && _playerLook != null;


        protected override void Awake()
        {
            base.Awake();
            _playerLook = GetComponentInParent<PlayerLook>();
        }

        protected virtual void LateUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }

            _playerLook.SensitivityScaleModifier.SetComponentValue(this, GetLookSensitivityScale());
        }

        protected override void OnUnequip()
        {
            base.OnUnequip();
            
            if (_playerLook != null)
            {
                _playerLook.SensitivityScaleModifier.RemoveComponent(this);
            }
        }

        protected virtual float GetLookSensitivityScale()
        {
            return Data != null ? Data.LookSensitivityScale : 1f;
        }
    }
}