using System;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField]
    private Transform bulletTarget;

    [SerializeField]
    private WeakPoint targetWeakPoint;

    public Action OnTargetDestroyed;

    private void OnEnable()
    {
        targetWeakPoint.OnHit += TargetDestroyed;
        targetWeakPoint.OnCrush += TargetDestroyed;
    }

    private void OnDisable()
    {
        targetWeakPoint.OnHit -= TargetDestroyed;
        targetWeakPoint.OnCrush -= TargetDestroyed;
    }

    public Transform GetTarget()
    {
        return bulletTarget;
    }

    private void TargetDestroyed(object sender, object e)
    {
        OnTargetDestroyed?.Invoke();
    }
}
