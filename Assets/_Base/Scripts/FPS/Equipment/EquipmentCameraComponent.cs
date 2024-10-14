using System;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    public abstract class EquipmentCameraComponent<T> : EquipmentComponent where T : Component
    {
        [SerializeField] private string _worldCameraObjectId = "world-camera";
        [SerializeField] private string _firstPersonCameraObjectId = "fp-camera";
        
        protected T _worldCameraComponent;
        protected T _firstPersonCameraComponent;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            if (!string.IsNullOrEmpty(_worldCameraObjectId))
            {
                _worldCameraComponent = GameObjectID.FindComponentInGameObjectWithID<T>(_worldCameraObjectId);
            }

            if (!string.IsNullOrEmpty(_firstPersonCameraObjectId))
            {
                _firstPersonCameraComponent = GameObjectID.FindComponentInGameObjectWithID<T>(_firstPersonCameraObjectId);
            }
        }

        protected virtual void LateUpdate()
        {
            if (!CanUpdate)
            {
                return;
            }

            if (_worldCameraComponent != null)
            {
                UpdateCameraComponent(_worldCameraComponent);
            }
            
            if (_firstPersonCameraComponent != null)
            {
                UpdateCameraComponent(_firstPersonCameraComponent);
            }
        }
        
        protected override void OnUnequip()
        {
            base.OnUnequip();
            
            if (_worldCameraComponent != null)
            {
                ResetCameraComponent(_worldCameraComponent);
            }
            
            if (_firstPersonCameraComponent != null)
            {
                ResetCameraComponent(_firstPersonCameraComponent);
            }
        }

        protected abstract void UpdateCameraComponent(T cameraComponent);

        protected abstract void ResetCameraComponent(T cameraComponent);
    }
}