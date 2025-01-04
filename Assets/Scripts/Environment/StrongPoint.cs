using System;
using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.8f;

    public event EventHandler OnHit;

    public void ProjectileHit(out float velocityConservation)
    {
        OnHit?.Invoke(this, null);

        velocityConservation = this.velocityConservation;
    }
}
