using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLockOn : RewindableAction
{
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private BulletLockOn bulletLockOn;

    public static void BulletLockedOn(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        new StartLockOn(target, bulletLockOn, initialPosition, initialDirection);
    }

    public StartLockOn(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        this.bulletLockOn = bulletLockOn;
        bulletLockOn.SetTarget(target);

        this.initialPosition = initialPosition;
        this.initialDirection = initialDirection;
    }

    public override void Undo()
    {
        bulletLockOn.UndoLockOn(initialPosition, initialDirection);
    }
}
