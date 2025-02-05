using System;
using UnityEngine;

public class ImpactEffectFuture : ImpactEffect
{
    public static EventHandler<Vector3> OnHitEnvironment;

    private void Start()
    {
        OnHitEnvironment?.Invoke(this, transform.position);
    }
}
