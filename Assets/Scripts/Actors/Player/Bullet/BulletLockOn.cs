using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLockOn : MonoBehaviour
{
    private LockOnTarget bulletTarget;

    private void TryLockOn() { }

    public void SetTarget(LockOnTarget newTarget)
    {
        if (bulletTarget)
        {
            bulletTarget.OnTargetDestroyed -= DisableLockOn;
        }
        bulletTarget = newTarget;
        bulletTarget.OnTargetDestroyed += DisableLockOn;
    }

    private void DisableLockOn()
    {
        //Disable lock on when target is destroyed
    }

    public void UndoLockOn(Vector3 initialPosition, Vector3 initialDirection)
    {
        SetTarget(null);
        //Bullet Movement change travel direction and position
    }

    public void RestartLockOn(
        LockOnTarget oldTarget,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        SetTarget(oldTarget);
        //Bullet Movement change travel direction and position
    }
}
