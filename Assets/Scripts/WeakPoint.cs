using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour, IDamageable
{
    public event Action OnHit;

    public void ProjectileHit(Bullet projectile)
    {
        OnHit?.Invoke();
    }

    public void ProjectileHit(MeleeWeapon weapon)
    {
        weapon.WeaponImpact();
        OnHit?.Invoke();
    }
}
