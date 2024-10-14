namespace FireRingStudio.FPS.ProjectileWeapon
{
    public class ProjectileWeaponRecoil : ProjectileWeaponComponent
    {
        private Recoil _recoil;

        private Recoil Recoil
        {
            get
            {
                if (_recoil == null)
                {
                    _recoil = GetComponentInParent<Recoil>();
                }

                return _recoil;
            }
        }


        protected override void OnEquip()
        {
            base.OnEquip();
            InitializeRecoil();
        }

        protected override void OnUse()
        {
            base.OnUse();
            AddRecoil();
        }

        private void InitializeRecoil()
        {
            if (Recoil == null || Data == null)
            {
                return;
            }
            
            Recoil.Snappiness = Data.RecoilSnappiness;
            Recoil.ReturnSpeed = Data.RecoilReturnSpeed;
        }

        private void AddRecoil()
        {
            if (Data == null)
            {
                return;
            }
            
            if (Recoil != null)
            {
                Recoil.AddRecoil(Data.RecoilForce);
            }

            if (Aim != null)
            {
                Aim.AddRecoil(Data.PrecisionRadiusRecoil);
            }
        }
    }
}