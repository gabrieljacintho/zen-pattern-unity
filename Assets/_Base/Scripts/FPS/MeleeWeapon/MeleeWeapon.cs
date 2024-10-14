using FireRingStudio.Vitals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.MeleeWeapon
{
    public class MeleeWeapon : UseEquipment.UseEquipment
    {
        [Header("Melee Weapon")]
        [SerializeField] private DamageArea _damageArea;
        [HideIf("@_damageArea != null")]
        [SerializeField] private string _damageAreaId;
        
        private MeleeWeaponData _meleeWeaponData;
        
        public new MeleeWeaponData Data
        {
            get
            {
                if (_meleeWeaponData == null)
                {
                    _meleeWeaponData = base.Data as MeleeWeaponData;
                }

                return _meleeWeaponData;
            }
        }
        private DamageArea DamageArea
        {
            get
            {
                if (_damageArea == null)
                {
                    _damageArea = ComponentID.FindComponentWithID<DamageArea>(_damageAreaId, true);
                }

                return _damageArea;
            }
        }
        
        
        protected override void OnEquip()
        {
            base.OnEquip();
            InitializeDamageArea();
        }
        
        protected override void OnUnequip()
        {
            base.OnUnequip();            
            DisableDamage();
        }
        
        protected override void OnStartUsing()
        {
            base.OnStartUsing();
            DisableDamage();
        }
        
        protected override void OnUse()
        {
            EnableDamage();
            
            float disableDelay = Data != null ? Data.DisableDamageDelay : 0.1f;
            Invoke(nameof(DisableDamage), disableDelay);
        }

        private void InitializeDamageArea()
        {
            if (DamageArea != null && Data != null)
            {
                DamageArea.DamageParameters = Data.DamageParameters;
            }
        }
        
        private void EnableDamage()
        {
            if (DamageArea != null)
            {
                DamageArea.gameObject.SetActive(true);
            }
        }
        
        private void DisableDamage()
        {
            if (DamageArea != null)
            {
                DamageArea.gameObject.SetActive(false);
            }
        }
    }
}