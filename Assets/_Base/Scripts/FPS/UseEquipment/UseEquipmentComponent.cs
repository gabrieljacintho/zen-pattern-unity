using FireRingStudio.FPS.Equipment;
using UnityEngine;

namespace FireRingStudio.FPS.UseEquipment
{
    [RequireComponent(typeof(UseEquipment))]
    public class UseEquipmentComponent : EquipmentComponent
    {
        private UseEquipment _useEquipment;

        protected override Equipment.Equipment Equipment => UseEquipment;
        protected UseEquipment UseEquipment
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
        protected new UseEquipmentData Data => _useEquipment.Data;


        protected override void Awake()
        {
            base.Awake();
            UseEquipment.OnStartUsingEvent.AddListener(OnStartUsing);
            UseEquipment.StopUseAction += OnStopUsing;
            UseEquipment.OnUseEvent.AddListener(OnUse);
            UseEquipment.EndUseAction += OnEndUsing;
        }
        
        protected virtual void OnStartUsing() { }
        
        protected virtual void OnStopUsing() { }

        protected virtual void OnUse() { }
        
        protected virtual void OnEndUsing() { }
    }
}