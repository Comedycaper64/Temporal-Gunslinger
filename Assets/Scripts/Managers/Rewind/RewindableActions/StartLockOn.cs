using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLockOn : RewindableAction
{
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private BulletLockOn bulletLockOn;

    public static void BulletLockedOn(
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection
    )
    {
        new StartLockOn(bulletLockOn, initialPosition, initialDirection);
    }

    public StartLockOn(BulletLockOn bulletLockOn, Vector3 initialPosition, Vector3 initialDirection)
    {
        this.bulletLockOn = bulletLockOn;

        this.initialPosition = initialPosition;
        this.initialDirection = initialDirection;

        Execute();
    }

    public override void Undo()
    {
        bulletLockOn.UndoLockOn(initialPosition, initialDirection);
    }
}
