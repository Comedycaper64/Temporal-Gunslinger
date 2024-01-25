using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.6f;
    public event Action OnHit;

    public void ProjectileHit(out float velocityConservation, out bool bIsPassable) //Bullet projectile)
    {
        OnHit?.Invoke();
        velocityConservation = this.velocityConservation;
        bIsPassable = true;
    }

    // public void ProjectileHit(MeleeWeapon weapon)
    // {
    //     weapon.WeaponImpact();
    //     OnHit?.Invoke();
    // }
}
