using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    public class EquipmentComponent : MonoBehaviour
    {
        private Equipment _equipment;
        
        protected virtual Equipment Equipment
        {
            get
            {
                if (_equipment == null)
                {
                    _equipment = GetComponentInParent<Equipment>();
                }

                return _equipment;
            }
        }
        protected EquipmentData Data => Equipment.Data;
        protected bool IsEquipped => Equipment.IsEquipped;
        protected virtual bool CanUpdate => !GameManager.IsPaused && Equipment.IsEquipped;


        protected virtual void Awake()
        {
            if (Equipment == null)
            {
                Debug.LogError("Equipment not found!", this);
                enabled = false;
                return;
            }
            
            Equipment.OnEquipEvent.AddListener(OnEquip);
            Equipment.OnUnequipEvent.AddListener(OnUnequip);
            
            if (Equipment.IsEquipped)
            {
                OnEquip();                
            }
            else
            {
                OnUnequip();
            }
        }
        
        protected virtual void OnEquip() { }
        
        protected virtual void OnUnequip() { }
    }
}