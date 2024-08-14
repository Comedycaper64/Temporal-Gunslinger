using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLockOn : RewindableAction
{
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private LockOnTarget lockOnTarget;
    private BulletLockOn bulletLockOn;

    public static void BulletLockOnFinished(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        new FinishLockOn(target, bulletLockOn, initialPosition, initialDirection);
    }

    public FinishLockOn(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        this.bulletLockOn = bulletLockOn;
        bulletLockOn.SetTarget(null);

        lockOnTarget = target;
        this.initialPosition = initialPosition;
        this.initialDirection = initialDirection;
    }

    public override void Undo()
    {
        bulletLockOn.RestartLockOn(lockOnTarget, initialPosition, initialDirection);
    }
}
