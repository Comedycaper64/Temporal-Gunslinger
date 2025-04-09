using System;
using UnityEngine;

public class ImpactEffectFuture : ImpactEffect
{
    public static EventHandler<Vector3> OnHitEnvironment;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnHitEnvironment?.Invoke(this, transform.position);
    }
}
