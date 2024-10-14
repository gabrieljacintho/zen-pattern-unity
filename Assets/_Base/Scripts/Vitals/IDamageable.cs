namespace FireRingStudio.Vitals
{
    public interface IDamageable
    {
        float CurrentHealth { get; }

        void TakeDamage(Damage damage);
    }
}
