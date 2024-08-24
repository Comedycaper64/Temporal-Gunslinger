using Cinemachine;
using UnityEngine;

public class BulletDamager : MonoBehaviour
{
    private bool bBulletActive;
    private CinemachineImpulseSource impulseSource;

    [SerializeField]
    private BulletMovement bulletMovement;

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private GameObject ricochetVFX;

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
            //Debug.Log("Hit: " + damageable);
            impulseSource.GenerateImpulse();

            damageable.ProjectileHit(out float velocityConservation);

            bulletMovement.SlowBullet(velocityConservation);
        }
    }

    public void SetBulletActive(bool bBulletActive)
    {
        this.bBulletActive = bBulletActive;
    }
}
