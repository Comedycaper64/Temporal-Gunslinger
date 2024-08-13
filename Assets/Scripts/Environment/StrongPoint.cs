using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.8f;

    public void ProjectileHit(out float velocityConservation)
    {
        //projectile.BulletImpact();
        velocityConservation = this.velocityConservation;
    }

    // public void ProjectileHit(MeleeWeapon weapon)
    // {
    //     weapon.WeaponImpact();
    // }
}
