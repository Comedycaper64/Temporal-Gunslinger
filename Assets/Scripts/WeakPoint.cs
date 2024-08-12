using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.6f;
    public event EventHandler OnHit;
    public event EventHandler OnCrush;

    public void ProjectileHit(out float velocityConservation)
    {
        OnHit?.Invoke(this, null);
        velocityConservation = this.velocityConservation;
    }

    public void EnvironmentCrush()
    {
        OnCrush?.Invoke(this, null);
    }

    // public void ProjectileHit(MeleeWeapon weapon)
    // {
    //     weapon.WeaponImpact();
    //     OnHit?.Invoke();
    // }
}
