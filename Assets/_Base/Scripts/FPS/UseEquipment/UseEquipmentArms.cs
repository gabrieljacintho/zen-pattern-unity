using FireRingStudio.FPS.Equipment;
using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentArms : EquipmentArms
    {
        private UseEquipment _useEquipment;
        
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


        protected override void Awake()
        {
            base.Awake();
            UseEquipment.OnStartUsingEvent.AddListener(OnStartUsing);
        }
        
        private void OnStartUsing()
        {
            if (ArmsOffset == null)
            {
                return;
            }
            
            UpdateArmsOffset();
            ArmsOffset.ApplyOffset();
        }
    }
}