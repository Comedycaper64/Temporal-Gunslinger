using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField]
    private Transform bulletTarget;

    public Action OnTargetDestroyed;

    public Transform GetTarget()
    {
        return bulletTarget;
    }

    private void TargetDestroyed()
    {
        OnTargetDestroyed?.Invoke();
    }
}
