using System;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.FPS.ProjectileWeapon
{
    [DisallowMultipleComponent]
    public class ProjectileWeaponCorrector : ProjectileWeaponComponent
    {
        [Serializable]
        public struct MovingPart
        {
            public Transform Transform;
            public Vector3 PositionOffset;
            public Vector3 RotationOffset;
        }

        [SerializeField] private MovingPart[] _movingParts;
        [SerializeField] private Transform[] _cartridgeTransforms;

        private ProjectileWeaponReload _reload;
        private Cartridge[] _cartridges;

        private bool IsReloading => _reload != null && _reload.IsReloading;
        private int QuantityOfAmmoInClip => ProjectileWeapon.AmmoClip?.Size ?? 0;
        private int QuantityOfAmmoInInventory =>ProjectileWeapon != null ? ProjectileWeapon.QuantityOfAmmoInInventory : 0;
        protected int TotalQuantityOfAmmo => QuantityOfAmmoInClip + QuantityOfAmmoInInventory;


        protected override void Awake()
        {
            base.Awake();
            _reload = GetComponent<ProjectileWeaponReload>();
            LoadCartridges();
            DisableCartridges();
            FillAmmoClip();
            
            if (_reload != null)
            {
                _reload.OnStartReload.AddListener(FillAmmoClip);
            }
        }

        protected virtual void LateUpdate()
        {
            if (GameManager.IsPaused || !IsEquipped)
            {
                return;
            }

            if (ProjectileWeapon.AmmoClip != null && ProjectileWeapon.AmmoClip.IsEmpty && !IsReloading)
            {
                MoveParts();
            }
        }

        private void LoadCartridges()
        {
            if (_cartridgeTransforms == null || Data == null)
            {
                return;
            }

            ProjectileData projectileData = Data.ProjectileData;
            Cartridge cartridgePrefab = projectileData != null ? projectileData.CartridgePrefab : null;
            if (cartridgePrefab == null)
            {
                return;
            }
            
            _cartridges = new Cartridge[_cartridgeTransforms.Length];

            for (int i = 0; i < _cartridgeTransforms.Length; i++)
            {
                cartridgePrefab.gameObject.GetInvisible(_cartridgeTransforms[i]).TryGetComponent(out _cartridges[i]);
            }
        }

        private void MoveParts()
        {
            if (_movingParts == null)
            {
                return;
            }

            for (int i = 0; i < _movingParts.Length; i++)
            {
                if (_movingParts[i].PositionOffset != Vector3.zero)
                {
                    _movingParts[i].Transform.localPosition += _movingParts[i].PositionOffset;
                }

                if (_movingParts[i].RotationOffset != Vector3.zero)
                {
                    _movingParts[i].Transform.localEulerAngles += _movingParts[i].RotationOffset;
                }
            }
        }

        protected void Fill(int quantity)
        {
            if (_cartridges == null)
            {
                return;
            }

            for (int i = 0; i < _cartridges.Length; i++)
            {
                if (i < quantity)
                {
                    _cartridges[i].Fill();
                }
                else
                {
                    _cartridges[i].EmptyOut();
                }
            }
        }
        
        #region AnimationEvents

        private void FillAmmoClip()
        {
            Fill(QuantityOfAmmoInClip);
        }
        
        protected virtual void FillCartridges()
        {
            if (_cartridges == null)
            {
                return;
            }
            
            int n = 0;
            for (int i = _cartridges.Length - 1; i >= 0; i--)
            {
                if (n < TotalQuantityOfAmmo)
                {
                    _cartridges[i].Fill();
                    n++;
                }
                else
                {
                    _cartridges[i].SetEnabled(false);
                }
            }
        }

        private void InvertCartridges()
        {
            if (_cartridges == null)
            {
                return;
            }
            
            for (int i = 0; i < _cartridges.Length; i++)
            {
                if (i < TotalQuantityOfAmmo)
                {
                    _cartridges[i].Fill();
                }
                else
                {
                    _cartridges[i].SetEnabled(false);
                }
            }
        }

        private void DisableCartridges()
        {
            if (_cartridges != null)
            {
                foreach (Cartridge cartridge in _cartridges)
                {
                    cartridge.SetEnabled(false);
                }
            }
        }
        
        #endregion
    }
}