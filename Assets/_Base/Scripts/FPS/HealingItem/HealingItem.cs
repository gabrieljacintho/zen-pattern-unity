using FireRingStudio.Vitals;

namespace FireRingStudio.FPS.HealingItem
{
    public class HealingItem : UseEquipment.UseEquipment
    {
        private Health _health;
        private HealingItemData _healingItemData;
        
        public new HealingItemData Data
        {
            get
            {
                if (_healingItemData == null)
                {
                    _healingItemData = base.Data as HealingItemData;
                }

                return _healingItemData;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            _health = GetComponentInParent<Health>();
        }

        protected override void OnUse()
        {
            if (Data == null || Data.PlayerHealthReference == null)
            {
                return;
            }

            float heal = UnityEngine.Random.Range(Data.MinHeal, Data.MaxHeal);
            Data.PlayerHealthReference.Value += heal;

            if (Data.CanHealPoison && _health != null)
            {
                _health.ClearExtendedDamages(); // TODO: Clear only poison damage
            }
        }
        
        protected override bool CanUse()
        {
            bool value = base.CanUse();
            
            if (Data == null || Data.PlayerHealthReference == null)
            {
                return false;
            }
            
            value &= Data.CanBeUsedWhenHealthIsFull || Data.PlayerHealth < Data.MaxPlayerHealth;

            return value;
        }
    }
}