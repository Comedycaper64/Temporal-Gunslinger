using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamager : MonoBehaviour
{
    private bool bBulletActive;

    [SerializeField]
    private BulletMovement bulletMovement;

    private void OnCollisionEnter(Collision other)
    {
        if (!bBulletActive)
        {
            return;
        }

        if (bulletMovement.IsBulletReversing())
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ProjectileHit(out float velocityConservation, out bool bIsPassable);
            bulletMovement.RicochetBullet(other, velocityConservation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bBulletActive)
        {
            return;
        }

        if (bulletMovement.IsBulletReversing())
        {
            return;
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            //Debug.Log("Hit: " + damageable);
            damageable.ProjectileHit(out float velocityConservation, out bool bIsPassable);

            bulletMovement.SlowBullet(velocityConservation);
        }
    }

    public void SetBulletActive(bool bBulletActive)
    {
        this.bBulletActive = bBulletActive;
    }
}
