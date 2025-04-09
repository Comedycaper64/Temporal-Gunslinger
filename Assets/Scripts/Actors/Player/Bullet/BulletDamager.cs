using System;
using Cinemachine;
using MoreMountains.Tools;
using UnityEngine;

public class BulletDamager : MonoBehaviour
{
    private bool bBulletActive;

    [SerializeField]
    private bool bFragileBullet;
    private CinemachineImpulseSource impulseSource;

    [SerializeField]
    private LayerMask collisionLayermask;

    [SerializeField]
    private BulletMovement bulletMovement;

    [SerializeField]
    private BulletStateMachine bulletStateMachine;

    public EventHandler<IDamageable> OnHitAchievementCheck;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

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
            if (!collisionLayermask.MMContains(other.gameObject.layer))
            {
                return;
            }

            if (bFragileBullet)
            {
                bulletStateMachine.SwitchToDeadState();
                return;
            }

            Vector3 impactNormal = other.GetContact(0).normal;
            Quaternion impactRotation = Quaternion.LookRotation(-impactNormal);

            RicochetManager.SpawnRicochetVFX(transform, impactRotation);

            RicochetManager.SpawnRicochetImpact(other);
            impulseSource.GenerateImpulse();

            damageable.ProjectileHit(out float velocityConservation);
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
            if (!collisionLayermask.MMContains(other.gameObject.layer))
            {
                return;
            }

            //Debug.Log("Hit: " + damageable);
            impulseSource.GenerateImpulse();

            OnHitAchievementCheck?.Invoke(this, damageable);

            damageable.ProjectileHit(out float velocityConservation, bulletMovement.GetVelocity());

            if (bFragileBullet)
            {
                bulletStateMachine.SwitchToDeadState();
                return;
            }

            bulletMovement.SlowBullet(velocityConservation);
        }
    }

    public void SetBulletActive(bool bBulletActive)
    {
        this.bBulletActive = bBulletActive;
    }
}
