using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.8f;

    public void ProjectileHit(out float velocityConservation, out bool bIsPassable) //Bullet projectile)
    {
        //projectile.BulletImpact();
        velocityConservation = this.velocityConservation;
        bIsPassable = false;
    }

    // public void ProjectileHit(MeleeWeapon weapon)
    // {
    //     weapon.WeaponImpact();
    // }
}
