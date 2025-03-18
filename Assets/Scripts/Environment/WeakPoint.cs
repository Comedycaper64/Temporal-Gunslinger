using System;
using UnityEngine;

public class WeakPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.6f;
    public event EventHandler OnHit;
    public event EventHandler OnCrush;

    public void ProjectileHit(out float velocityConservation, float bulletSpeed = 0f)
    {
        OnHit?.Invoke(this, null);

        velocityConservation = this.velocityConservation;
    }

    public void EnvironmentCrush()
    {
        //Debug.Log("Crush");
        OnCrush?.Invoke(this, null);
    }
}
