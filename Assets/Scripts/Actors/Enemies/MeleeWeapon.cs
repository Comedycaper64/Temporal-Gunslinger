using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public void WeaponImpact() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(out float velocityConservation);
        }
    }
}
