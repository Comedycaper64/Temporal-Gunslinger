using UnityEngine;

public class StartLockOn : RewindableAction
{
    private bool persistent;
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private BulletLockOn bulletLockOn;

    public static void BulletLockedOn(
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection,
        bool persistent = false
    )
    {
        new StartLockOn(bulletLockOn, initialPosition, initialDirection, persistent);
    }

    public StartLockOn(
        BulletLockOn bulletLockOn,
        Vector3 initialPosition,
        Vector3 initialDirection,
        bool persistent
    )
    {
        this.bulletLockOn = bulletLockOn;

        this.initialPosition = initialPosition;
        this.initialDirection = initialDirection;
        this.persistent = persistent;

        Execute();
    }

    public override void Undo()
    {
        bulletLockOn.UndoLockOn(initialPosition, initialDirection, persistent);
    }
}
