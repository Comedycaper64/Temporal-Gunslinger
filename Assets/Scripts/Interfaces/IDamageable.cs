public interface IDamageable
{
    void ProjectileHit(out float velocityConservation, float bulletSpeed = 0f);
}
