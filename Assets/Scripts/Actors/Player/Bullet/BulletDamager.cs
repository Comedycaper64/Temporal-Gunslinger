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

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private GameObject ricochetVFX;

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

            Factory.InstantiateGameObject(ricochetVFX, transform.position, transform.rotation);
            impulseSource.GenerateImpulse();

            Vector3 impactPoint = other.GetContact(0).point;
            Vector3 impactNormal = other.GetContact(0).normal;

            GameObject impact = Factory.InstantiateGameObject(
                impactEffect,
                other.gameObject.transform
            );
            impact.transform.position = impactPoint + 0.1f * impactNormal.normalized;
            impact.transform.rotation = Quaternion.LookRotation(-impactNormal);

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
