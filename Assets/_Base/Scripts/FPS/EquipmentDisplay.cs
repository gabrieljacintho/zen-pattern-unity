using FireRingStudio.FPS.UseEquipment;
using FireRingStudio.Inventory;
using UnityEngine;

namespace FireRingStudio.FPS
{
    public class EquipmentDisplay : MonoBehaviour
    {
        [SerializeField] private GameObjectSwitcher _equipmentDisplaySwitcher;
        [SerializeField] private GameObjectSwitcher _useDisplaySwitcher;
        [SerializeField] private string _firstPersonCameraId;

        private EquipmentSwapper _equipmentSwapper;
        private Camera _firstPersonCamera;

        private Camera FirstPersonCamera
        {
            get
            {
                if (_firstPersonCamera == null && !string.IsNullOrEmpty(_firstPersonCameraId))
                {
                    _firstPersonCamera = ComponentID.FindComponentWithID<Camera>(_firstPersonCameraId);
                }

                return _firstPersonCamera;
            }
        }


        private void Awake()
        {
            _equipmentSwapper = GameObjectID.FindComponentInChildrenWithID<EquipmentSwapper>(GameObjectID.PlayerID, true);
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            Equipment.Equipment selectedEquipment = _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;

            UpdateEquipmentDisplay(selectedEquipment);
            UpdateUseDisplay(selectedEquipment);
        }

        private void UpdateEquipmentDisplay(Equipment.Equipment equipment)
        {
            if (_equipmentDisplaySwitcher == null)
            {
                return;
            }

            bool canShow = false;
            string id = null;
            if (equipment != null)
            {
                canShow = equipment.CanShowUserInterface;
                if (canShow)
                {
                    id = equipment.Data.UserInterfaceID;
                }
            }

            if (canShow)
            {
                _equipmentDisplaySwitcher.SwitchGameObject(id);
            }
            else
            {
                _equipmentDisplaySwitcher.ResetGameObject();
            }
        }

        private void UpdateUseDisplay(Equipment.Equipment equipment)
        {
            if (_useDisplaySwitcher == null)
            {
                return;
            }

            bool canShow = false;
            string id = null;

            if (equipment is ProjectileWeapon.ProjectileWeaponBase projectileWeapon)
            {
                bool isAiming = projectileWeapon.IsAiming;
                ItemData scopeData = projectileWeapon.Data != null ? projectileWeapon.Data.ScopeData : null;

                if (scopeData != null)
                {
                    id = scopeData.Id;
                    canShow = isAiming && scopeData != null;
                    canShow &= (projectileWeapon.Aim != null && projectileWeapon.Aim.CanShowScope) || id == _useDisplaySwitcher.CurrentObjectId;
                }
            }

            if (!canShow && equipment is UseEquipment.UseEquipment useEquipment)
            {
                id = useEquipment.Data.UseUserInterfaceID;
                canShow = useEquipment.CanShowUseUserInterface && !string.IsNullOrEmpty(id);
            }

            if (canShow)
            {
                _useDisplaySwitcher.SwitchGameObject(id);
            }
            else
            {
                _useDisplaySwitcher.ResetGameObject();
            }

            if (FirstPersonCamera != null)
            {
                _firstPersonCamera.enabled = !canShow;
            }
        }
    }
}