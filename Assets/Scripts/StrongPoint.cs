using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    public void ProjectileHit(Bullet projectile)
    {
        projectile.BulletImpact();
    }

    public void ProjectileHit(MeleeWeapon weapon)
    {
        weapon.WeaponImpact();
    }
}
