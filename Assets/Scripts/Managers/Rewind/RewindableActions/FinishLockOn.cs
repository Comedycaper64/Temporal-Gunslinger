using UnityEngine;

public class FinishLockOn : RewindableAction
{
    private bool persistent;
    private float lockOnRotationAmount;
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private Vector3 lockOnStartDirection;
    private LockOnTarget lockOnTarget;
    private BulletLockOn bulletLockOn;

    public static void BulletLockOnFinished(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        float lockOnRotationAmount,
        Vector3 initialPosition,
        Vector3 initialDirection,
        Vector3 lockOnStartDirection,
        bool persistent = false
    )
    {
        new FinishLockOn(
            target,
            bulletLockOn,
            lockOnRotationAmount,
            initialPosition,
            initialDirection,
            lockOnStartDirection,
            persistent
        );
    }

    public FinishLockOn(
        LockOnTarget target,
        BulletLockOn bulletLockOn,
        float lockOnRotationAmount,
        Vector3 initialPosition,
        Vector3 initialDirection,
        Vector3 lockOnStartDirection,
        bool persistent
    )
    {
        this.bulletLockOn = bulletLockOn;
        lockOnTarget = target;
        this.lockOnRotationAmount = lockOnRotationAmount;
        this.initialPosition = initialPosition;
        this.initialDirection = initialDirection;
        this.lockOnStartDirection = lockOnStartDirection;
        this.persistent = persistent;

        Execute();
    }

    public override void Undo()
    {
        bulletLockOn.RestartLockOn(
            lockOnTarget,
            lockOnRotationAmount,
            initialPosition,
            initialDirection,
            lockOnStartDirection,
            persistent
        );
    }
}
