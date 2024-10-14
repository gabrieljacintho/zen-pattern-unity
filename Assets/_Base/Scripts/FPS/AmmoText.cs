using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.FPS
{
    public class AmmoText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _ammoInWeaponText;
        [SerializeField] private TextMeshProUGUI _ammoInInventoryText;
        [SerializeField] private string _separatorText = "/";

        private Image _image;
        private EquipmentSwapper _equipmentSwapper;


        private void Awake()
        {
            _image = GetComponent<Image>();
            _equipmentSwapper = GameObjectID.FindComponentInChildrenWithID<EquipmentSwapper>(GameObjectID.PlayerID, true);
        }

        private void LateUpdate()
        {
            if (_ammoInWeaponText == null && _ammoInInventoryText == null)
            {
                SetEnable(false);
                return;
            }
            
            Equipment.Equipment currentEquipment = _equipmentSwapper != null ? _equipmentSwapper.CurrentEquipment : null;
            if (currentEquipment == null || currentEquipment is not ProjectileWeapon.ProjectileWeaponBase projectileWeapon)
            {
                SetEnable(false);
                return;
            }

            if (_ammoInWeaponText != null)
            {
                int ammoStackSizeInGun = projectileWeapon.AmmoClip?.Size ?? 0;
                _ammoInWeaponText.text = ammoStackSizeInGun.ToString();
            }
            
            if (_ammoInInventoryText != null)
            {
                if (_ammoInWeaponText != null)
                {
                    _ammoInWeaponText.text += _separatorText;
                }
                
                int ammoPackSize = projectileWeapon.QuantityOfAmmoInInventory;
                if (_ammoInInventoryText == _ammoInWeaponText)
                {
                    _ammoInWeaponText.text += ammoPackSize;
                }
                else
                {
                    _ammoInInventoryText.text = ammoPackSize.ToString();
                }
            }
            
            SetEnable(true);
        }

        private void SetEnable(bool value)
        {
            if (_image != null)
            {
                _image.enabled = value;
            }

            if (_ammoInWeaponText != null)
            {
                _ammoInWeaponText.enabled = value;
            }

            if (_ammoInInventoryText != null)
            {
                _ammoInInventoryText.enabled = value;
            }
        }
    }
}