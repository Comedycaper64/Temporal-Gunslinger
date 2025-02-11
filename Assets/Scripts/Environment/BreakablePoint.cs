using System;
using UnityEngine;

public class BreakablePoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.8f;
    public event EventHandler<float> OnHit;

    public void ProjectileHit(out float velocityConservation, float bulletSpeed = 0f)
    {
        OnHit?.Invoke(this, bulletSpeed);

        velocityConservation = this.velocityConservation;
    }
}
